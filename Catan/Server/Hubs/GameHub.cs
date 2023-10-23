using Catan.Shared.Model;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using Database;
using Database.Data;
using Microsoft.AspNetCore.Authorization;
using Catan.Shared.Request;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Catan.Shared.Response;
using BLL.Services.Interfaces;
using BLL.Services;
using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Dice;
using Newtonsoft.Json.Linq;

namespace Catan.Server.Hubs
{
    [Authorize]
	public class GameHub : Hub
	{
		private readonly IGameService _gameService;
		public GameHub(IGameService gameService)
		{
			_gameService = gameService;
		}
		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}
		public async Task JoinGroup(string groupName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
		}
		public async Task LeaveGroup(string groupName)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
		}
		public async Task SaveConnectionId(Actor actor, string conId, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			var success = _gameService.RegisterPlayerConnectionId(Guid.Parse(guidstring), actor.Name, conId);
			if (success== GameServiceResponses.GameCanStart)
			{
				Guid guid = Guid.Parse(guidstring);
				_gameService.StartGame(guid);
				await Clients.Group(guid.ToString()).SendAsync("ProcessCurrentPlayer",GetCurrentPlayer(guidstring));
				await CallNextPlayer(guid);
			}
		}
		public async Task EndPlayerTurn(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name !=GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't end your turn during someone else's turn");
			}
			Guid guid = Guid.Parse(guidstring);
			_gameService.EndPlayerTurn(guid,actor.Name);
			await Clients.Caller.SendAsync("TurnEnded");
			await CallNextPlayer(guid);
		}
		public int[] GetLatestRolledBaseDices(string guidstring)
		{
			Guid guid = Guid.Parse(guidstring);
			var dices= _gameService.GetLastRolledDices(guid);
			if (dices is null)
			{
				throw new Exception("null");
			}
			List<int> res = new List<int>();
			foreach (var dice in dices)
			{
				if (dice.DiceType == typeof(int))
				{
					res.Add(Convert.ToInt32(dice.Value));
				}
			}
			return res.ToArray();
		}
		public async Task RollDices(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name != GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't roll dices during someone else's turn");
			}
			Guid guid = Guid.Parse(guidstring);
			var response = _gameService.RollDices(guid, actor.Name); //TODO 

			var dices = GetLatestRolledBaseDices(guidstring);
			if (dices is not null)
			{
				int sum = 0;
				foreach (var dice in dices)
				{
					sum += dice;
				}
				if (sum == 7)
				{
					ResolveDiceRollSeven(guid);
				}
				await Clients.Caller.SendAsync("DiceRolled");
				await Clients.Group(guid.ToString()).SendAsync("ProcessDiceRolled");
				await Clients.Group(guid.ToString()).SendAsync("FetchResources");
			}
			else
			{
				throw new Exception("Something went wrong with dice rolling");
			}
			
		}
		private void ResolveDiceRollSeven(Guid guid)
		{
			var conIDsWithSevenOrMoreResources = _gameService.GetPlayersConnectionIdWithSevenOrMoreResources(guid);
			if (conIDsWithSevenOrMoreResources is not null)
			{
				foreach (var connection in conIDsWithSevenOrMoreResources)
				{
					Clients.Client(connection).SendAsync("ResolveSevenRoll"); // TODO kliens oldalon kezelni, hogy nyersanyagot eldobjon
				}
			}
			else
			{
				throw new Exception("Something went wrong with getting the players with 7 or more resources");
			}
			var conId = _gameService.GetActivePlayerConnectionId(guid);
			if (conId is not null)
			{
				Clients.Client(conId).SendAsync("ResolveRobberMovement");
			}
			else
			{
				throw new Exception("Can't find active player");
			}
			//TODO a felező algoritmus visszatér az eldobott nyersanyagokkal, sszerver oldalon ellenőrizni, hogy tényleg jó mennyiséget dobott-e el
			//ha igen, ha minden pacek, akkor pedig mindenkinek frissíti a játékot.
		}
		public async Task ThrowResourcesOnSevenRoll(Actor actor, string guidstring, Inventory inventory)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid=Guid.Parse(guidstring);
			var response = _gameService.ThrowResources(guid, inventory, actor.Name);
			await NotifyClients(Guid.Parse(guidstring));
		}
		public string GetMap(string guidstring)
		{
			Guid guid = Guid.Parse(guidstring);
			var map = _gameService.GetGameMap(guid);
			if (map is null)
			{
				throw new Exception("map is null");
			}
			var options = new JsonSerializerOptions
			{
				MaxDepth = 1000,
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
				IncludeFields=true
			};
			string res= JsonSerializer.Serialize(map, options);
			return res;
		}
		public string? GetCurrentPlayer(string guidstring)
		{
			Guid guid = Guid.Parse(guidstring);
			var res = _gameService.GetActivePlayerName(guid);
			if (res is not null)
			{
				return res;
			}
			return null;
		}
		public async Task CallNextPlayer(Guid guid)
		{
			var connection = _gameService.GetActivePlayerConnectionId(guid);
			if (connection is null)
			{
				return;
			}
			if (_gameService.IsGameOver(guid) == GameServiceResponses.GameOver)
			{
				await Clients.Group(guid.ToString()).SendAsync("GameOver");
			}
			else if (_gameService.IsInitialRound(guid) == GameServiceResponses.InitialRound)
			{
				await Clients.Client(connection).SendAsync("PlaceInitialVillage");
			}
			else
			{
				await Clients.Client(connection).SendAsync("TakeNormalTurn");
			}
		}
		public async Task ClaimInitialVillage(Actor actor,string guidstring,int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid= Guid.Parse(guidstring);
			if (_gameService.IsInitialRound(guid)==GameServiceResponses.NotInitialRound)
			{
				throw new Exception("It's not starting round");
			}
			var playerName = _gameService.GetActivePlayerName(guid);
			if (playerName is null)
			{
				throw new Exception("active player is null");
			}
            if (playerName.CompareTo(actor.Name)==0)
			{
				try
				{
					var response = _gameService.BuildInitialVillage(guid, id, actor.Name); //TODO
                    await NotifyMapChanged(guid);
					await NotifyClients(guid);
					await Clients.Caller.SendAsync("PlaceInitialRoad");
				}
				catch (Exception e)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", e.Message);
				}
			}
		}
		public async Task ClaimInitialRoad(Actor actor, string guidstring,int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid= Guid.Parse(guidstring);
			if (_gameService.IsInitialRound(guid) == GameServiceResponses.NotInitialRound)
			{
				throw new Exception("It's not starting round");
			}
            var playerName = _gameService.GetActivePlayerName(guid);
            if (playerName is null)
            {
                throw new Exception("active player is null");
            }
            if (playerName == actor.Name)
			{
				try
				{
					var response = _gameService.BuildInitialRoad(guid, id, actor.Name); //TODO
                    await NotifyMapChanged(guid);
					await Clients.Caller.SendAsync("InitialTurnDone");
				}
				catch (Exception e)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", e.Message);
					return;
				}
				var response2 = _gameService.EndPlayerTurn(guid, actor.Name);//TODO
				await CallNextPlayer(guid);
				await NotifyClients(guid);
			}
		}
		private async Task NotifyClients(Guid guid)
		{
			await Clients.Group(guid.ToString()).SendAsync("ProcessCurrentPlayer",GetCurrentPlayer(guid.ToString()));
			await Clients.Group(guid.ToString()).SendAsync("FetchResources");
			await Clients.Group(guid.ToString()).SendAsync("FetchTradeOffers"); //TODO külön hívásba
		}
		private async Task NotifyMapChanged(Guid guid)
		{
			await Clients.Group(guid.ToString()).SendAsync("ProcessMap", GetMap(guid.ToString()));
		}
		public async Task ClaimCorner(Actor actor, string guidstring, int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			if (_gameService.IsInitialRound(guid) == GameServiceResponses.InitialRound)
			{
				throw new Exception("It's starting round");
			}
			var playerName = _gameService.GetActivePlayerName(guid);
			if (playerName is null)
			{
				throw new Exception("active player is null");
			}
            if (playerName  == actor.Name)
			{
				try
				{
					var response = _gameService.BuildVillage(guid, id, actor.Name); //TODO
					await NotifyMapChanged(guid);
					await NotifyClients(guid);
				}
				catch (Exception e)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", e.Message);
				}
			}
		}
		public async Task ClaimEdge(Actor actor, string guidstring, int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			if (_gameService.IsInitialRound(guid) == GameServiceResponses.InitialRound)
			{
				throw new Exception("It's starting round");
			}
			var playerName = _gameService.GetActivePlayerName(guid);
			if (playerName is null)
			{
				throw new Exception("active player is null");
			}
            if (playerName == actor.Name)
			{
				try
				{
					var response = _gameService.BuildRoad(guid, id, actor.Name); //TODO
					await NotifyMapChanged(guid);
					await NotifyClients(guid);
				}
				catch (Exception e)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", e.Message);
					return;
				}
			}
		}
		public FetchInventoryDTO GetPlayersInventories(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			FetchInventoryDTO result=new FetchInventoryDTO();
			result.Inventory = _gameService.GetPlayersInventory(guid,actor.Name) ?? throw new Exception("refactorme, no inventory");
			result.OthersInventory=_gameService.GetOtherPlayersInventory(guid,actor.Name) ?? throw new Exception("refactorme, no other inventory");
			return result;
		}
		public List<Player> GetPlayerList(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			return _gameService.GetPlayers(guid) ?? throw new Exception("refactorme, playerlist is null");
		}
		public async Task MoveRobber(Actor actor, string guidstring, int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			var playerName = _gameService.GetActivePlayerName(guid);
			if (playerName is null)
			{
				throw new Exception("active player is null");
			}
            if (playerName == actor.Name)
			{
				try
				{
					_gameService.MoveRobber(guid, id, actor.Name!); //TODO
					await NotifyMapChanged(guid);
					await NotifyClients(guid);
					await Clients.Caller.SendAsync("RobberMovementResolved");
				}
				catch (Exception e)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", e.Message);
				}
			}
		}
		

		public List<TradeOffer>? GetTradeOffers(string guidstring) //TODO kliensoldalról még indítani kell 
		{
			return _gameService.GetTradeOffers(Guid.Parse(guidstring));
		}
		public async Task SendTradeOffer(Actor actor, string guidstring, TradeOffer tradeOffer) //TODO kliens oldalról még indítani kell
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name != GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't send trade offers during someone else's turn");
			}
			if (tradeOffer.ToPlayers)
			{
				var response = _gameService.RegisterTradeOffer(Guid.Parse(guidstring), tradeOffer); //TODO
			}
			else
			{
				var response = _gameService.RegisterTradeOfferWithBank(Guid.Parse(guidstring), tradeOffer); //TODO
			}
			
			await NotifyClients(Guid.Parse(guidstring));
		}
		public async Task AcceptTradeOffer(Actor actor, string guidstring, TradeOffer tradeOffer) //TODO kliens oldalról még indítani kell
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name == GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't accept trade offers during your turn");
			}
			var response = _gameService.AcceptTradeOffer(Guid.Parse(guidstring), tradeOffer, actor.Name); //TODO
			await NotifyClients(Guid.Parse(guidstring));
		}
	}
}

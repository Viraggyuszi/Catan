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
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Model.GameMap;
using Microsoft.Extensions.Options;
using Azure;

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
			if (success == GameServiceResponses.GameCanStart)
			{
				Guid guid = Guid.Parse(guidstring);
				_gameService.StartGame(guid); //TODO response handling
				await Clients.Group(guid.ToString()).SendAsync("ProcessCurrentPlayer", GetCurrentPlayer(guidstring));
				await CallNextPlayer(guid);
			}
		}
		public async Task EndPlayerTurn(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", "Using other player's name");
				return;
			}
			if (actor.Name != GetCurrentPlayer(guidstring))
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", "You can't end your turn during someone else's turn");
				return;
			}
			Guid guid = Guid.Parse(guidstring);
			var response = _gameService.EndPlayerTurn(guid, actor.Name);
			if (response != GameServiceResponses.Success)
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
				return;
			}
			await Clients.Caller.SendAsync("TurnEnded");
			await CallNextPlayer(guid);
		}
		public int[] GetLatestRolledBaseDices(string guidstring)
		{
			Guid guid = Guid.Parse(guidstring);
			var dices = _gameService.GetLastRolledDices(guid);
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
				await Clients.Caller.SendAsync("ProcessErrorMessage", "Using other player's name");
				return;
			}
			if (actor.Name != GetCurrentPlayer(guidstring))
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", "You can't end your turn during someone else's turn");
				return;
			}
			Guid guid = Guid.Parse(guidstring);
			var response = _gameService.RollDices(guid, actor.Name);
			if (response != GameServiceResponses.Success)
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
				return;
			}
			var dices = GetLatestRolledBaseDices(guidstring) ?? throw new Exception("Something went wrong with getting the rolled dices");
			if (dices.Sum()==7)
			{
				ResolveDiceRollSeven(guid);
			}
			await Clients.Caller.SendAsync("DiceRolled");
			await Clients.Group(guid.ToString()).SendAsync("ProcessDiceRolled");
			await Clients.Group(guid.ToString()).SendAsync("FetchResources");
		}
		private void ResolveDiceRollSeven(Guid guid) //TODO
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
			//TODO a felező algoritmus visszatér az eldobott nyersanyagokkal, szerver oldalon ellenőrizni, hogy tényleg jó mennyiséget dobott-e el
			//ha igen, ha minden pacek, akkor pedig mindenkinek frissíti a játékot.
		}
		public async Task ThrowResourcesOnSevenRoll(Actor actor, string guidstring, AbstractInventory inventory)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			var response = _gameService.ThrowResources(guid, inventory, actor.Name);
			if (response != GameServiceResponses.Success)
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
				return;
			}
			await NotifyClients(Guid.Parse(guidstring));
		}
		public Map GetMap(string guidstring)
		{
			Guid guid = Guid.Parse(guidstring);
			var map = _gameService.GetGameMap(guid);
			if (map is null)
			{
				throw new Exception("map is null");
			}
			return map;
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
		public async Task BuildInitialVillage(Actor actor, string guidstring, int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			if (_gameService.IsInitialRound(guid) == GameServiceResponses.NotInitialRound)
			{
				throw new Exception("It's not starting round");
			}
			var playerName = _gameService.GetActivePlayerName(guid) ?? throw new Exception("active player is null");
			if (playerName.CompareTo(actor.Name) == 0)
			{
				var response = _gameService.BuildInitialVillage(guid, id, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await NotifyClients(guid);
				await Clients.Caller.SendAsync("PlaceInitialRoad");
			}
		}
		public async Task BuildInitialRoad(Actor actor, string guidstring, int id)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
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
				var response = _gameService.BuildInitialRoad(guid, id, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await Clients.Caller.SendAsync("InitialTurnDone");
				response = _gameService.EndPlayerTurn(guid, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					throw new Exception("Something went wrong with turn ending");
				}
				await CallNextPlayer(guid);
				await NotifyClients(guid);
			}
		}
		private async Task NotifyClients(Guid guid)
		{
			await Clients.Group(guid.ToString()).SendAsync("ProcessCurrentPlayer", GetCurrentPlayer(guid.ToString()));
			await Clients.Group(guid.ToString()).SendAsync("FetchResources");
		}
		private async Task NotifyTradeOffersChanged(Guid guid)
		{
			await Clients.Group(guid.ToString()).SendAsync("FetchTradeOffers");
		}
		private async Task NotifyMapChanged(Guid guid)
		{
			await Clients.Group(guid.ToString()).SendAsync("ProcessMap", GetMap(guid.ToString()));
		}
		public async Task BuildVillage(Actor actor, string guidstring, int id)
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
				var response = _gameService.BuildVillage(guid, id, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await NotifyClients(guid);
			}
		}
		public async Task BuildCity(Actor actor, string guidstring, int id)
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
			if (playerName != actor.Name)
			{
				throw new Exception("Not Active player");
			}
			var response = _gameService.BuildCity(guid, id, actor.Name);
			if (response != GameServiceResponses.Success)
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
				return;
			}
			await NotifyMapChanged(guid);
			await NotifyClients(guid);
		}
		public async Task BuildRoad(Actor actor, string guidstring, int id)
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
			var playerName = _gameService.GetActivePlayerName(guid) ?? throw new Exception("active player is null");
			if (playerName == actor.Name)
			{
				var response = _gameService.BuildRoad(guid, id, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await NotifyClients(guid);
			}
		}
		public async Task BuildShip(Actor actor, string guidstring, int id)
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
			var playerName = _gameService.GetActivePlayerName(guid) ?? throw new Exception("active player is null");
			if (playerName == actor.Name)
			{
				var response = _gameService.BuildShip(guid, id, actor.Name);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await NotifyClients(guid);
			}
		}
		public FetchInventoryDTO GetPlayersInventories(Actor actor, string guidstring)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			Guid guid = Guid.Parse(guidstring);
			FetchInventoryDTO result = new FetchInventoryDTO();
			result.Inventory = _gameService.GetPlayersInventory(guid, actor.Name) ?? throw new Exception("refactorme, no inventory");
			result.OthersInventory = _gameService.GetOtherPlayersInventory(guid, actor.Name) ?? throw new Exception("refactorme, no other inventory");
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
				var response = _gameService.MoveRobber(guid, id, actor.Name!);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyMapChanged(guid);
				await NotifyClients(guid);
				await Clients.Caller.SendAsync("RobberMovementResolved");
			}
		}
		public List<TradeOffer>? GetTradeOffers(string guidstring)
		{
			var tradeOffers = _gameService.GetTradeOffers(Guid.Parse(guidstring));
			return tradeOffers;
		}
		public async Task SendTradeOffer(Actor actor, string guidstring, TradeOffer tradeOffer)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name != GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't send trade offers during someone else's turn");
			}
			if (tradeOffer is null)
			{
				throw new Exception("Trade offer is null");
			}
			if (tradeOffer.ToPlayers)
			{
				var response = _gameService.RegisterTradeOffer(Guid.Parse(guidstring), tradeOffer);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
				await NotifyTradeOffersChanged(new Guid(guidstring));
			}
			else
			{
				var response = _gameService.RegisterTradeOfferWithBank(Guid.Parse(guidstring), tradeOffer);
				if (response != GameServiceResponses.Success)
				{
					await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
					return;
				}
			}
			await NotifyClients(Guid.Parse(guidstring));
		}
		public async Task AcceptTradeOffer(Actor actor, string guidstring, TradeOffer tradeOffer)
		{
			if (!ActorIdentity.CheckActorIdentity(actor))
			{
				throw new Exception("Using other player's name");
			}
			if (actor.Name == GetCurrentPlayer(guidstring))
			{
				throw new Exception("You can't accept trade offers during your turn");
			}
			if (tradeOffer is null)
			{
				throw new Exception("Trade offer is null");
			}
			var response = _gameService.AcceptTradeOffer(Guid.Parse(guidstring), tradeOffer, actor.Name);
			if (response != GameServiceResponses.Success)
			{
				await Clients.Caller.SendAsync("ProcessErrorMessage", response.ToString());
				return;
			}
			await NotifyClients(Guid.Parse(guidstring));
			await NotifyTradeOffersChanged(new Guid(guidstring));
		}
	}
}

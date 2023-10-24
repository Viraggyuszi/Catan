using BLL.GameActions;
using BLL.Services.Interfaces;
using BLL.Services.MapServices;
using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Dice;
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Response;

namespace BLL.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IInMemoryDatabaseGame _inMemoryDatabaseGame;
        private readonly IMapService _mapService;
		private Dictionary<Game, GameActionHandler> _gameHandlers;
        private readonly GameActionHandlerFactory _gameActionHandlerFactory;
        public GameService(IInMemoryDatabaseGame inMemoryDatabaseGame, IMapService mapService)
        {
            _inMemoryDatabaseGame = inMemoryDatabaseGame;
            _mapService = mapService;
			_gameHandlers = new Dictionary<Game, GameActionHandler>();
			_gameActionHandlerFactory = new GameActionHandlerFactory();
        }
        public InMemoryDatabaseGameResponses RegisterGame(Guid guid, Game game)
        {
            var newMap = _mapService.GenerateMap(game.DLCs);
            game.GameMap = newMap;
			_gameHandlers.Add(game, _gameActionHandlerFactory.CreateActionHandler(game.DLCs));
            return _inMemoryDatabaseGame.AddGame(guid, game);
        }
		public GameServiceResponses StartGame(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			game.InitialRound = true;
			game.InitialRoundCount = game.PlayerList.Count * 2;
			game.ActivePlayer = game.PlayerList[new Random().Next(1, game.PlayerList.Count)];
			return GameServiceResponses.Success;
		}
		public Map? GetGameMap(Guid guid)
        {
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return null;
            }
            return game.GameMap;
        }
        public GameServiceResponses RegisterPlayerConnectionId(Guid guid, string name, string connectionId)
        {
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
            var player = game.PlayerList.FirstOrDefault(x => x.Name == name);
            if (player is null)
            {
                return GameServiceResponses.InvalidMember;
            }
            player.ConnectionID = connectionId;

            if (game.PlayerList.All(p => p.ConnectionID is not null) && !game.AlreadyInitialized)
            {
                game.AlreadyInitialized = true;
                return GameServiceResponses.GameCanStart;
            }
            return GameServiceResponses.GameCantStart;
        }
		public List<string>? GetPlayersConnectionIdWithSevenOrMoreResources(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			var res = new List<string>();
			foreach (var player in game.PlayersWithSevenOrMoreResources)
			{
				res.Add(player.ConnectionID!);
			}
			return res;
		}
		public string? GetActivePlayerConnectionId(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			return game.ActivePlayer.ConnectionID;
		}
		public string? GetActivePlayerName(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			return game.ActivePlayer.Name;
		}
		public List<IDice>? GetLastRolledDices(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			return game.Dices;
		}
		public GameServiceResponses IsInitialRound(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return game.InitialRound ? GameServiceResponses.InitialRound : GameServiceResponses.NotInitialRound;
		}
		public Dictionary<Resources, int>? GetPlayersInventory(Guid guid, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			var player = game.PlayerList.Find(p => p.Name == name);
			if (player is null)
			{
				return null;
			}
			return player.Inventory.GetResources();
		}
		public Dictionary<string, int>? GetOtherPlayersInventory(Guid guid, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			Dictionary<string, int> res = new Dictionary<string, int>();
			foreach (var player in game.PlayerList)
			{
				if (player.Name is not null && player.Name != name)
				{
					var number = player.Inventory.GetAllResourcesCount();
					res.Add(player.Name, number);
				}
			}
			return res;
		}
		public GameServiceResponses IsGameOver(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			if (game.PlayerList.Any(p => p.Points >= game.PointsToWin))
			{
				game.Winner = game.PlayerList.First(p => p.Points >= game.PointsToWin);
				return GameServiceResponses.GameOver;
			}
			return GameServiceResponses.GameInProgress;
		}
		public List<Player>? GetPlayers(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			return game.PlayerList;
		}
		public List<TradeOffer>? GetTradeOffers(Guid guid)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return null;
			}
			return game.TradeOfferList;
		}
		public GameServiceResponses EndPlayerTurn(Guid guid, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteEndTurnAction(game, name);
		}
		public GameServiceResponses BuildVillage(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteBuildVillageAction(game, id, name);
		}
		public GameServiceResponses BuildCity(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteBuildCityAction(game, id, name);
		}
		public GameServiceResponses BuildRoad(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteBuildRoadAction(game, id, name);
		}
		public GameServiceResponses BuildInitialVillage(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteBuildInitialVillageAction(game, id, name);
		}
		public GameServiceResponses BuildInitialRoad(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteBuildInitialRoadAction(game, id, name);
		}
		public GameServiceResponses MoveRobber(Guid guid, int id, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteMoveRobberAction(game, id, name);
		}
		public GameServiceResponses RegisterTradeOfferWithBank(Guid guid, TradeOffer offer) // TODO  configolható lehessen tengeri városoknál a kereskedés aránya (1:2 vagy 1:3 akár)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteRegisterTradeOfferWithBankAction(game, offer);
		}
		public GameServiceResponses RegisterTradeOffer(Guid guid, TradeOffer offer)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteRegisterTradeOfferAction(game, offer);
		}
		public GameServiceResponses AcceptTradeOffer(Guid guid, TradeOffer offer, string name)
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteAcceptTradeOfferAction(game, offer, name);
		}
		public GameServiceResponses ThrowResources(Guid guid, AbstractInventory thrownResources, string name) //TODO idk mit???
		{
			var game = _inMemoryDatabaseGame.GetGame(guid);
			if (game is null)
			{
				return GameServiceResponses.InvalidGame;
			}
			return _gameHandlers[game].ExecuteThrowResourcesAction(game, thrownResources, name);
		}
		public GameServiceResponses RollDices(Guid guid, string name)
        {
            var game = _inMemoryDatabaseGame.GetGame(guid);
            if (game is null)
            {
                return GameServiceResponses.InvalidGame;
            }
			return _gameHandlers[game].ExecuteDiceRollAction(game, name);
        }
    }
}

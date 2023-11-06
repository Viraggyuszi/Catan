using BLL.GameActions;
using BLL.Services.Interfaces;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class InMemoryDatabaseGame : IInMemoryDatabaseGame
    {
        private ConcurrentDictionary<Guid, Game> GuidGamePairs { get; set; }
        public InMemoryDatabaseGame()
        {
            GuidGamePairs = new ConcurrentDictionary<Guid, Game>();
            GameGameHandlerPair = new ConcurrentDictionary<Game, GameActionHandler>();
        }
        public InMemoryDatabaseGameResponses AddGame(Guid guid, Game game, GameActionHandler handler)
        {
            AddGameHandler(game, handler);
            return GuidGamePairs.TryAdd(guid, game) ? InMemoryDatabaseGameResponses.Success : InMemoryDatabaseGameResponses.CreateGameFailed;
        }
        public InMemoryDatabaseGameResponses RemoveGame(Guid guid)
        {
            var removeResult = GuidGamePairs.TryRemove(guid, out var game);
            if (game is not null)
            {
				RemoveGameHandler(game);
			}
			return removeResult ? InMemoryDatabaseGameResponses.Success : InMemoryDatabaseGameResponses.RemoveGameFailed;
        }
        public Game? GetGame(Guid guid)
        {
            return GuidGamePairs.GetValueOrDefault(guid);
        }

		private ConcurrentDictionary<Game, GameActionHandler> GameGameHandlerPair { get; set; }
		private InMemoryDatabaseHandlerResponses AddGameHandler(Game game, GameActionHandler handler)
		{
			return GameGameHandlerPair.TryAdd(game, handler) ? InMemoryDatabaseHandlerResponses.Success : InMemoryDatabaseHandlerResponses.RegisterHandlerFailed;
		}
		private InMemoryDatabaseHandlerResponses RemoveGameHandler(Game game)
		{
			return GameGameHandlerPair.TryRemove(game, out _) ? InMemoryDatabaseHandlerResponses.Success : InMemoryDatabaseHandlerResponses.RemoveHandlerFailed;
		}
		public GameActionHandler? GetGameActionHandler(Game game)
		{
			return GameGameHandlerPair.GetValueOrDefault(game);
		}


	}
}

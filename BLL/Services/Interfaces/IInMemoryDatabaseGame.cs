using BLL.GameActions;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IInMemoryDatabaseGame
    {
        public InMemoryDatabaseGameResponses AddGame(Guid guid, Game game, GameActionHandler handler);
        public InMemoryDatabaseGameResponses RemoveGame(Guid guid);
        public Game? GetGame(Guid guid);
		public GameActionHandler? GetGameActionHandler(Game game);
	}
}

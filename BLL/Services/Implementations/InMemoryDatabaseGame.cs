using BLL.Services.Interfaces;
using Catan.Shared.Model;
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
        }
        public InMemoryDatabaseGameResponses AddGame(Guid guid, Game game)
        {
            return GuidGamePairs.TryAdd(guid, game) ? InMemoryDatabaseGameResponses.Success : InMemoryDatabaseGameResponses.CreateGameFailed;
        }
        public InMemoryDatabaseGameResponses RemoveGame(Guid guid)
        {
            return GuidGamePairs.TryRemove(guid, out _) ? InMemoryDatabaseGameResponses.Success : InMemoryDatabaseGameResponses.RemoveGameFailed;
        }
        public Game? GetGame(Guid guid)
        {
            return GuidGamePairs.GetValueOrDefault(guid);
        }
    }
}

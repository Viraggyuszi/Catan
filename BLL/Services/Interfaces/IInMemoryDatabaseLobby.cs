using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IInMemoryDatabaseLobby
    {
        public InMemoryDatabaseLobbyResponses AddLobby(Guid guid, Lobby lobby);
        public InMemoryDatabaseLobbyResponses RemoveLobby(Guid guid);
        public InMemoryDatabaseLobbyResponses AddPlayerToLobby(Player player, Guid guid);
        public InMemoryDatabaseLobbyResponses RemovePlayerFromLobby(Player player, Guid guid);
        public List<Lobby> GetLobbies();
        public Game? CreateGame(Guid guid);
    }
}

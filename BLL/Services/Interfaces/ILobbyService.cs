using Catan.Shared.Model;
using Catan.Shared.Model.GameState;
using Catan.Shared.Request;
using Catan.Shared.Response;

namespace BLL.Services.Interfaces
{
    public interface ILobbyService
    {
        public InMemoryDatabaseLobbyResponses CreateLobby(string name, Dictionary<GameType, bool> DLCs);
        public InMemoryDatabaseLobbyResponses AddPlayerToLobby(Player player, Guid guid);
        public InMemoryDatabaseLobbyResponses RemovePlayerFromLobby(Player player, Guid guid);
        public InMemoryDatabaseGameResponses StartLobbyGame(Guid guid);
        public List<Lobby> GetAllLobbies();
    }
}

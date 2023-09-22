using Catan.Shared.Model;
using Catan.Shared.Request;
using Catan.Shared.Response;

namespace BLL.Interfaces
{
	public interface ILobbyService
	{
		public InMemoryDatabaseLobbyResponses CreateLobby(string name);
		public InMemoryDatabaseLobbyResponses AddPlayerToLobby(Player player, Guid guid);
		public InMemoryDatabaseLobbyResponses RemovePlayerFromLobby(Player player, Guid guid);
		public InMemoryDatabaseGameResponses StartLobbyGame(Guid guid);
		public List<Lobby> GetAllLobbies();
	}
}

using Catan.Shared.Model;
using Catan.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IGameService
	{
		public Lobby CreateLobby(string name);
		public ApiDTO<string> RegisterGame(Guid guid, Map map);
		public Game GetGame(Guid guid);
		public List<Lobby> GetLobbies();
		public ApiDTO<string> AddPlayerToLobby(Player player, Guid guid);
		public ApiDTO<string> RemovePlayerFromLobby(Player player, Guid guid);
		public ApiDTO<int[]> RollDices(Guid guid);
		public bool RegisterPlayerConnectionId(Guid guid, string name, string connectionId);
		public ApiDTO<string[]> GetPlayersConnectionIdWithSevenOrMoreResources(Guid guid);
		public string GetActivePlayerConnectionId(Guid guid);
		public Player GetActivePlayer(Guid guid);

		public bool IsInitialRound(Guid guid);
		public void NextTurn(Guid guid);

		public void StartGame(Guid guid);
		public void ClaimInitialCorner(Guid guid, int id, string name);
		public void ClaimInitialRoad(Guid guid, int id, string name);

		public void ClaimCorner(Guid guid, int id, string name);
		public void ClaimEdge(Guid guid, int id, string name);

		public void MoveRobber(Guid guid, int id, string name);


		public Dictionary<Resources, int> GetPlayersInventory(Guid guid, string name);
		public Dictionary<string, int> GetOtherPlayersInventory(Guid guid, string name);

		public bool IsGameOver(Guid guid);

		public List<Player> GetPlayers(Guid guid);
	}
}

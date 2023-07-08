using Catan.Shared.Model;
using Catan.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface ILobbyService
	{
		public Lobby createLobby(string name);
		public ApiDTO<string> addPlayerToLobby(Player player, Guid guid);
		public ApiDTO<string> removePlayerFromLobby(Player player, Guid guid);
		public ApiDTO<string> StartLobbyGame(Guid guid);
		public List<Lobby> GetAllLobby();
	}
}

using BLL.Interfaces;
using Catan.Shared.Model;
using Catan.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
	public class LobbyService : ILobbyService
	{
		private readonly IGameService _gameService;
		private readonly IMapService _mapService;
		public LobbyService(IGameService gameService, IMapService mapService)
		{
			_gameService = gameService;
			_mapService = mapService;
		}
		public Lobby createLobby(string name)
		{
			var lobby = _gameService.CreateLobby(name);
			return lobby;
		}

		public ApiDTO<string> addPlayerToLobby(Player player, Guid guid)
		{
			var ret = _gameService.AddPlayerToLobby(player, guid);
			return ret;
		}

		public ApiDTO<string> removePlayerFromLobby(Player player, Guid guid)
		{
			var ret = _gameService.RemovePlayerFromLobby(player, guid);
			return ret;
		}

		public ApiDTO<string> StartLobbyGame(Guid guid)
		{
			var NewGameMap = _mapService.GenerateMap();
			return _gameService.RegisterGame(guid, NewGameMap);
		}
		public List<Lobby> GetAllLobby()
		{
			return _gameService.GetLobbies();
		}
	}
}

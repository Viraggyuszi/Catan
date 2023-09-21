using BLL.Interfaces;
using Catan.Shared.Model;
using Catan.Shared.Request;
using Catan.Shared.Response;
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
		private readonly IInMemoryDatabaseLobby _inMemoryDatabaseLobby;
		public LobbyService(IGameService gameService, IInMemoryDatabaseLobby inMemoryDatabaseLobby )
		{
			_gameService = gameService;
			_inMemoryDatabaseLobby = inMemoryDatabaseLobby;
		}
		public InMemoryDatabaseLobbyResponses CreateLobby(string name)
		{
            var guid = Guid.NewGuid();
            var lobby = new Lobby
            {
                Name = name,
                Players = new List<Player>(),
                GuID = guid
            };
            return _inMemoryDatabaseLobby.AddLobby(guid, lobby);
        }

		public InMemoryDatabaseLobbyResponses AddPlayerToLobby(Player player, Guid guid)
		{
            return _inMemoryDatabaseLobby.AddPlayerToLobby(player, guid);
        }

		public InMemoryDatabaseLobbyResponses RemovePlayerFromLobby(Player player, Guid guid)
		{
			return _inMemoryDatabaseLobby.RemovePlayerFromLobby(player, guid);
        }

		public InMemoryDatabaseGameResponses StartLobbyGame(Guid guid)
		{
			var game =_inMemoryDatabaseLobby.CreateGame(guid);
			if (game is null)
			{
				return InMemoryDatabaseGameResponses.CreateGameFailed;
			}
			var response = _gameService.RegisterGame(guid, game);
			if (response==InMemoryDatabaseGameResponses.Success)
			{
				_inMemoryDatabaseLobby.RemoveLobby(guid);
			}
			return response;
		}
		public List<Lobby> GetAllLobbies()
		{
            return _inMemoryDatabaseLobby.GetLobbies();
        }
	}
}

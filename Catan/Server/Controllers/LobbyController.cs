using BLL.Services;
using BLL.Services.Interfaces;
using Catan.Shared.Model.GameState;
using Catan.Shared.Request;
using Catan.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catan.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;
        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

		[HttpPost("create")]
        public ApiDTO<InMemoryDatabaseLobbyResponses> CreateNewLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
        {
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
			var lobby= _lobbyService.CreateLobby(playerLobbyDTO.Lobby!.Name!);
            return new ApiDTO<InMemoryDatabaseLobbyResponses>() { Success = lobby == InMemoryDatabaseLobbyResponses.Success, Value = lobby }; //TODO kezelni kliensoldalon
        }

        [HttpGet("getall")]
        public ApiDTO<List<Lobby>> GetAllLobbies()
        {
            var lobbyList = _lobbyService.GetAllLobbies();
            return new ApiDTO<List<Lobby>>() {  Success = true, Value=lobbyList };
        }

        [HttpPost("join")]
        public ApiDTO<InMemoryDatabaseLobbyResponses> JoinLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
        {
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
            Player player = new Player { Name = playerLobbyDTO.Actor.Name };
			var response=_lobbyService.AddPlayerToLobby(player, playerLobbyDTO.Lobby.GuID);
			return new ApiDTO<InMemoryDatabaseLobbyResponses> { Success = response==InMemoryDatabaseLobbyResponses.Success, Value = response }; //TODO kezelni kliens oldalon
        }

        [HttpPost("leave")]
        public ApiDTO<InMemoryDatabaseLobbyResponses> LeaveLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
		{
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
			Player player = new Player { Name = playerLobbyDTO.Actor!.Name };
			var response = _lobbyService.RemovePlayerFromLobby(player, playerLobbyDTO.Lobby!.GuID);
            return new ApiDTO<InMemoryDatabaseLobbyResponses> { Success = response == InMemoryDatabaseLobbyResponses.Success, Value = response }; //TODO kezelni kliens oldalon
        }

        [HttpPost("start")]
		public ApiDTO<string> CreateGameFromLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
		{
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
			if (playerLobbyDTO.Lobby!.Players.Count<2)
            {
                return new ApiDTO<string> { Success = false, Value = "You need at least 2 players to play" };
            }
            if (playerLobbyDTO.Lobby.Players[0].Name==playerLobbyDTO.Actor!.Name)
            {
				var response = _lobbyService.StartLobbyGame(playerLobbyDTO.Lobby.GuID);
				return new ApiDTO<string> { Success=true, Value="Ok"};
			}
			return new ApiDTO<string> { Success = false, Value = "You must be the leader of the lobby to start the game" };
		}
	}
}

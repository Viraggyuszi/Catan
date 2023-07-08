using BLL.Implementations;
using BLL.Interfaces;
using Catan.Shared.Model;
using Catan.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ApiDTO<Lobby> CreateNewLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
        {
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
			var Lobby= _lobbyService.createLobby(playerLobbyDTO.Lobby!.Name!);
            return new ApiDTO<Lobby>() { Success = true, Value=Lobby };
        }

        [HttpGet("getall")]
        public ApiDTO<List<Lobby>> GetAllLobby()
        {
            var lobbyList = _lobbyService.GetAllLobby();
            return new ApiDTO<List<Lobby>>() {  Success = true, Value=lobbyList };
        }

        [HttpPost("join")]
        public ApiDTO<string> JoinLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
        {
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
            Player player = new Player { Name = playerLobbyDTO.Actor!.Name };
			var response=_lobbyService.addPlayerToLobby(player, playerLobbyDTO.Lobby!.GuID);
            return response;    
        }

        [HttpPost("leave")]
        public ApiDTO<string> LeaveLobby([FromBody] PlayerLobbyDTO playerLobbyDTO)
		{
			if (!ActorIdentity.CheckActorIdentity(playerLobbyDTO.Actor!))
			{
				throw new Exception("Using other player's name");
			}
			if (playerLobbyDTO.Lobby!.Players.Find(p=>p.Name==playerLobbyDTO.Actor!.Name) is null)
            {
				return new ApiDTO<string> { Success = false, Value = "You must be a member of the lobby to leave" };
			}
			Player player = new Player { Name = playerLobbyDTO.Actor!.Name };
			var response = _lobbyService.removePlayerFromLobby(player, playerLobbyDTO.Lobby.GuID);
			return response;
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
				return response;
			}
			return new ApiDTO<string> { Success = false, Value = "You must be the leader of the lobby to start the game" };
		}
	}
}

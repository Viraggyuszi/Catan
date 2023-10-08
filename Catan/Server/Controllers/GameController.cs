using Catan.Shared.Model;
using Catan.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Catan.Server.Hubs;
using Azure;
using System;
using BLL.Services.Interfaces;

namespace Catan.Server.Controllers
{
    [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class GameController : ControllerBase
	{
		private readonly IGameService _gameService;
		public GameController(IGameService gameService)
		{
			_gameService = gameService;
		}
	}
}

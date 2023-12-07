using BLL.Services.Implementations;
using Catan.Shared.Request;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _databaseService;
        public UserController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        [HttpPost("register")]
        public async Task<ApiDTO<string>> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO is null)
            {
                return new ApiDTO<string>() { Success = false, Value = "Invalid username or password" };
            }
            return await _databaseService.RegisterNewUser(registerDTO);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiDTO<string>>> Login([FromBody] LoginDTO loginDTO)
        { 
            if (loginDTO is null)
            {
                return new ApiDTO<string>() { Success = false, Value = "Gib username or password" };
            }
            return await _databaseService.LoginUser(loginDTO);
        }
    }
}

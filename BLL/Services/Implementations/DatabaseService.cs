using Database.Models;
using Catan.Shared.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class DatabaseService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public DatabaseService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ApiDTO<string>> RegisterNewUser(RegisterDTO registerDTO)
        {
            var user = new User { UserName = registerDTO.Username, Email = registerDTO.Email };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                return new ApiDTO<string>() { Success = true, Value = "OK" };
            }
            else
            {
                ApiDTO<string> ret = new ApiDTO<string>();
                ret.Success = false;
                string message = "";
                result.Errors.ToList().ForEach(a => message += a.Description + "\n");
                ret.Value = message;
                return ret;
            }
        }

        public async Task<ApiDTO<string>> LoginUser(LoginDTO loginDTO)
        {
            //var user = await _userManager.FindByNameAsync(loginDTO.Username);
            //if (user is null)
            //{
            //    return new ApiDTO<string>() { Success = false, Value = "User not found!" };
            //}
            //if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            //{
            //    return new ApiDTO<string>() { Success = false, Value = "Incorrect password!" };
            //}
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)), SecurityAlgorithms.HmacSha256),
                claims: new[] { new Claim("username", loginDTO.Username) }
                );
            return new ApiDTO<string>() { Success = true, Value = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}

using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var identity = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(7)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LogIn(LoginUserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var token = GenerateToken(new User { Name=user.Name, Email = user.Email}); ;
                    return Ok(new { Token = token });
                }
                ModelState.AddModelError("errors", "Invalid credentials");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "Hiiiii" });
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successfully" });
        }

        [HttpGet("tokens")]
        [Authorize]
        public async Task<ActionResult<int>> GetTokens(string userEmail)
        {
            var user = await  _userManager.FindByNameAsync(userEmail);
            if(user == null)
            {
                return BadRequest();
            }
            return user.Tokens;
        }

        //[HttpGet("tokens")]
        //[Authorize]
        //public async Task<int> GetTokens()
    }
}

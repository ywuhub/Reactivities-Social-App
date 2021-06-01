using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
      [ApiController]
      [Route("api/[controller]")]
      public class AccountController : ControllerBase
      {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly TokenService _tokenService;
            public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
            {
                  _tokenService = tokenService;
                  _signInManager = signInManager;
                  _userManager = userManager;
            }

            [HttpPost("login")]
            public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
            {
                  var user = await _userManager.FindByEmailAsync(loginDTO.Email);

                  if (user == null) return Unauthorized();

                  var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

                  if (result.Succeeded)
                  {
                        return new UserDTO
                        {
                              DisplayName = user.DisplayName,
                              Image = null,
                              Token = _tokenService.CreateToken(user),
                              Username = user.UserName
                        };
                  }

                  return Unauthorized();
            }
      }
}
using System.Threading.Tasks;
using API.DTOs;
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
            public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
            {
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
                        Token = "This will be a generated token",
                        Username = user.UserName
                    };
                }

                return Unauthorized();
            }
      }
}
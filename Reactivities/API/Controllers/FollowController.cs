using System.Threading.Tasks;
using Application.Followers;
using Microsoft.AspNetCore.Mvc;
using Reactivities.API.Controllers;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command{TargetUsername = username}));
        }
    }
}
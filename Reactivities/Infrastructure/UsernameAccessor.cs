using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
      public class UsernameAccessor : IUsernameAccessor
      {
            private readonly IHttpContextAccessor _httpContextAccessor;
            public UsernameAccessor(IHttpContextAccessor httpContextAccessor)
            {
                  _httpContextAccessor = httpContextAccessor;
            }

            public string GetUsername()
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
      }
}
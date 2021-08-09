using CarDiaryX.Application.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarDiaryX.Web.Services
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = httpContextAccessor
                .HttpContext
                ?.User
                ?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}

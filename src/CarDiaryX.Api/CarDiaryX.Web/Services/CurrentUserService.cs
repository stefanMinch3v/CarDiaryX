using CarDiaryX.Application.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarDiaryX.Web.Services
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            this.UserId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}

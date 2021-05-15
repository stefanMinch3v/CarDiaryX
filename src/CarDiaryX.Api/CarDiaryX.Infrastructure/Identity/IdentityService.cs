using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.Identity;
using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Identity
{
    internal class IdentityService : IIdentity
    {
        private const string INVALID_LOGIN_ERROR_MESSAGE = "Invalid credentials.";

        private readonly UserManager<User> userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public IdentityService(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginOutputModel>> Login(LoginUserCommand userInput)
        {
            var user = await this.userManager.FindByEmailAsync(userInput.Email);
            if (user is null)
            {
                return INVALID_LOGIN_ERROR_MESSAGE;
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, userInput.Password);
            if (!passwordValid)
            {
                return INVALID_LOGIN_ERROR_MESSAGE;
            }

            var (token, expiration) = this.jwtTokenGenerator.GenerateToken(user);

            return new LoginOutputModel(token, expiration);
        }

        public async Task<Result> Register(RegisterUserCommand userInput)
        {
            var user = new User(userInput.Email, userInput.FirstName, userInput.LastName, userInput.Age);

            var identityResult = await this.userManager.CreateAsync(user, userInput.Password);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }
    }
}

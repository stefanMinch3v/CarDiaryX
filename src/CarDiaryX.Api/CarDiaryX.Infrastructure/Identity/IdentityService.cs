using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Identity;
using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Identity
{
    internal class IdentityService : IIdentity
    {
        private readonly UserManager<User> userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly ICurrentUser currentUser;
        private readonly IPermissionRepository permissionRepository;

        public IdentityService(
            UserManager<User> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ICurrentUser currentUser,
            IPermissionRepository permissionRepository)
        {
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.currentUser = currentUser;
            this.permissionRepository = permissionRepository;
        }

        public async Task<Result<LoginOutputModel>> Login(LoginUserCommand request)
        {
            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return InfrastructureConstants.INVALID_LOGIN_ERROR_MESSAGE;
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return InfrastructureConstants.INVALID_LOGIN_ERROR_MESSAGE;
            }

            var (token, expiration) = this.jwtTokenGenerator.GenerateToken(user);

            return new LoginOutputModel(token, expiration);
        }

        public async Task<Result> Register(RegisterUserCommand request)
        {
            var user = new User(request.Email, request.FirstName, request.LastName);

            var identityResult = await this.userManager.CreateAsync(user, request.Password);

            var errors = identityResult.Errors.Select(e => e.Description);

            if (identityResult.Succeeded)
            {
                await this.permissionRepository.AddDefault(user.Id);
            }

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<Result> ChangePassword(ChangeUserPasswordCommand request)
        {
            var user = await this.userManager.FindByIdAsync(this.currentUser.UserId);

            if (user is null)
            {
                return InfrastructureConstants.UNEXISTING_USER;
            }

            var identityResult = await this.userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<Result> DeleteUser(DeleteUserCommand request)
        {
            // TODO: Clean all vehicles and any data connected to this account

            var user = await this.userManager.FindByIdAsync(this.currentUser.UserId);

            if (user is null)
            {
                return InfrastructureConstants.UNEXISTING_USER;
            }

            var isValidPassword = await this.userManager.CheckPasswordAsync(user, request.ConfirmPassword);

            if (!isValidPassword)
            {
                return InfrastructureConstants.INVALID_CONFIRM_PASSWORD_ERROR_MESSAGE;
            }

            var identityResult = await this.userManager.DeleteAsync(user);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<UserDetailsOutputModel> GetUserDetails(CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByIdAsync(this.currentUser.UserId);

            if (user is null)
            {
                return null;
            }

            return new UserDetailsOutputModel(user.Email, user.FirstName, user.LastName);
        }

        public async Task<Result> UpdateUserDetails(UpdateUserDetailsCommand request)
        {
            var user = await this.userManager.FindByIdAsync(this.currentUser.UserId);

            if (user is null)
            {
                return InfrastructureConstants.UNEXISTING_USER;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var identityResult = await this.userManager.UpdateAsync(user);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }
    }
}

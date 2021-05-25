using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity
{
    public interface IIdentity
    {
        Task<Result> Register(RegisterUserCommand request);

        Task<Result<LoginOutputModel>> Login(LoginUserCommand request);

        Task<Result> ChangePassword(ChangeUserPasswordCommand request);

        Task<Result> DeleteUser(DeleteUserCommand request);

        Task<UserDetailsOutputModel> GetUserDetails(CancellationToken cancellationToken);

        Task<Result> UpdateUserDetails(UpdateUserDetailsCommand request);
    }
}

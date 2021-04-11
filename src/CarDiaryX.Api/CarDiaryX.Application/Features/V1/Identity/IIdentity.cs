using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.Identity.Commands;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity
{
    public interface IIdentity
    {
        Task<Result> Register(RegisterUserCommand input);

        Task<Result<LoginOutputModel>> Login(LoginUserCommand request);
    }
}

using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class LoginUserCommand : IRequest<Result<LoginOutputModel>>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginOutputModel>>
        {
            private readonly IIdentity identity;

            public LoginUserCommandHandler(IIdentity identity)
                => this.identity = identity;

            public Task<Result<LoginOutputModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
                => this.identity.Login(request);
        }
    }
}

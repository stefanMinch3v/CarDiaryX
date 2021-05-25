using CarDiaryX.Application.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class ChangeUserPasswordCommand : IRequest<Result>
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Result>
        {
            private readonly IIdentity identity;

            public ChangeUserPasswordCommandHandler(IIdentity identity)
                => this.identity = identity;

            public Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
                => this.identity.ChangePassword(request);
        }
    }
}

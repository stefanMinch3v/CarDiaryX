using CarDiaryX.Application.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public string ConfirmPassword { get; set; }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
        {
            private readonly IIdentity identity;

            public DeleteUserCommandHandler(IIdentity identity)
            {
                this.identity = identity;
            }

            public Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
                => this.identity.DeleteUser(request);
        }
    }
}

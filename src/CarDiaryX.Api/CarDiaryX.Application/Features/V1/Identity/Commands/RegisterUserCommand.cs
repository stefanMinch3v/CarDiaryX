using CarDiaryX.Application.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class RegisterUserCommand : IRequest<Result>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
        {
            private readonly IIdentity identity;

            public RegisterUserCommandHandler(IIdentity identity)
                => this.identity = identity;

            public Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
                => this.identity.Register(request);
        }
    }
}

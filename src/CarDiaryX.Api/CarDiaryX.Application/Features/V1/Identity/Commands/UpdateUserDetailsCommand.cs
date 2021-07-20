using CarDiaryX.Application.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Commands
{
    public class UpdateUserDetailsCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand, Result>
        {
            private readonly IIdentity identity;

            public UpdateUserDetailsCommandHandler(IIdentity identity)
            {
                this.identity = identity;
            }

            public Task<Result> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
                => this.identity.UpdateUserDetails(request);
        }
    }
}

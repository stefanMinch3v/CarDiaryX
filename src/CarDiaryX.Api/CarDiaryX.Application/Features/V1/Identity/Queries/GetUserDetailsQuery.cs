using CarDiaryX.Application.Features.V1.Identity.OutputModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Identity.Queries
{
    public class GetUserDetailsQuery : IRequest<UserDetailsOutputModel>
    {
        internal class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsOutputModel>
        {
            private readonly IIdentity identity;

            public GetUserDetailsQueryHandler(IIdentity identity)
            {
                this.identity = identity;
            }

            public Task<UserDetailsOutputModel> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
                => this.identity.GetUserDetails(cancellationToken);
        }
    }
}

using CarDiaryX.Application.Contracts;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetAllRegistrationNumbersQuery : IRequest<object>
    {
        public class GetAllRegistrationNumbersQueryHandler : IRequestHandler<GetAllRegistrationNumbersQuery, object>
        {
            private readonly ICurrentUser currentUser;
            private readonly IRegistrationNumberRepository numberRepository;

            public GetAllRegistrationNumbersQueryHandler(ICurrentUser currentUser, IRegistrationNumberRepository numberRepository)
            {
                this.currentUser = currentUser;
                this.numberRepository = numberRepository;
            }

            public async Task<object> Handle(GetAllRegistrationNumbersQuery request, CancellationToken cancellationToken)
            {
                var result = await this.numberRepository.GetByUser(this.currentUser.UserId, cancellationToken);
                return result.Select(r => new { r.Number, r.ShortDescription });
            }
        }
    }
}

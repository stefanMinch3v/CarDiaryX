using CarDiaryX.Application.Features.V1.Vehicles.OutputModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetAllRegistrationNumbersQuery : IRequest<IReadOnlyCollection<RegistrationNumberOutputModel>>
    {
        internal class GetAllRegistrationNumbersQueryHandler : IRequestHandler<GetAllRegistrationNumbersQuery, IReadOnlyCollection<RegistrationNumberOutputModel>>
        {
            private readonly IRegistrationNumberRepository numberRepository;

            public GetAllRegistrationNumbersQueryHandler(IRegistrationNumberRepository numberRepository)
            {
                this.numberRepository = numberRepository;
            }

            public async Task<IReadOnlyCollection<RegistrationNumberOutputModel>> Handle(GetAllRegistrationNumbersQuery request, CancellationToken cancellationToken)
            {
                var result = await this.numberRepository.GetByUser(cancellationToken);

                return result
                    .Select(r => new RegistrationNumberOutputModel 
                    { 
                        Number = r.Number, 
                        ShortDescription = r.ShortDescription 
                    })
                    .ToArray();
            }
        }
    }
}

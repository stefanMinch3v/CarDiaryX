using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetVehicleQuery : IRequest<object>
    {
        public string RegistrationNumber { get; set; }

        public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, object>
        {
            private readonly IVehicleRepository vehicleRepository;

            public GetVehicleQueryHandler(IVehicleRepository vehicleRepository)
            {
                this.vehicleRepository = vehicleRepository;
            }

            public async Task<object> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
            {
                var result = await this.vehicleRepository.GetInformation(request.RegistrationNumber, cancellationToken);
                return result;
            }
        }
    }
}

using CarDiaryX.Application.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class RemoveVehicleFromUserCommand : IRequest<Result>
    {
        public string RegistrationNumber { get; set; }

        internal class RemoveVehicleFromUserCommandHandler : IRequestHandler<RemoveVehicleFromUserCommand, Result>
        {
            private readonly IVehicleRepository vehicleRepository;

            public RemoveVehicleFromUserCommandHandler(IVehicleRepository vehicleRepository) 
                => this.vehicleRepository = vehicleRepository;

            public async Task<Result> Handle(RemoveVehicleFromUserCommand request, CancellationToken cancellationToken)
            {
                await this.vehicleRepository.RemoveAllVehicleData(false, request.RegistrationNumber);
                return Result.Success;
            }
        }
    }
}

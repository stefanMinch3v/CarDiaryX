using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Features.V1.VehicleServices.InputModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Commands
{
    public class AddVehicleServiceCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressInputModel Address { get; set; }

        internal class AddVehicleServiceCommandHandler : IRequestHandler<AddVehicleServiceCommand, Result>
        {
            private readonly IVehicleServicesRepository vehicleServicesRepository;

            public AddVehicleServiceCommandHandler(IVehicleServicesRepository vehicleServicesRepository) 
                => this.vehicleServicesRepository = vehicleServicesRepository;

            public async Task<Result> Handle(AddVehicleServiceCommand request, CancellationToken cancellationToken)
            {
                var success = await this.vehicleServicesRepository.Add(request.Name, request.Description, request.Address);

                if (!success)
                {
                    return Result.Failure(new[] { ApplicationConstants.VehicleServices.INVALID_ALREADY_EXISTING_SERVICE });
                }

                return Result.Success;
            }
        }
    }
}

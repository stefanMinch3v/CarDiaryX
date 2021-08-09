using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class UpdateVehicleInspectionCommand : IRequest<Result>
    {
        public UpdateVehicleInspectionCommand(string registrationNumber, string userId)
        {
            this.RegistrationNumber = registrationNumber;
            this.UserId = userId;
        }

        public string RegistrationNumber { get; }
        public string UserId { get; }

        internal class UpdateVehicleInspectionCommandHandler : IRequestHandler<UpdateVehicleInspectionCommand, Result>
        {
            private readonly IServiceProvider serviceProvider;

            public UpdateVehicleInspectionCommandHandler(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            public async Task<Result> Handle(UpdateVehicleInspectionCommand request, CancellationToken cancellationToken)
            {
                using var scope = this.serviceProvider.CreateScope();

                var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
                var vehicleHttpService = scope.ServiceProvider.GetRequiredService<IVehicleHttpService>();

                var existingInspection = await vehicleRepository.GetInspection(request.RegistrationNumber, cancellationToken);

                if (existingInspection is null || !DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, existingInspection.CreatedOn))
                {
                    var (_, dataId) = await vehicleRepository.GetDataAndTsIds(request.RegistrationNumber, cancellationToken);
                    var rawData = await vehicleHttpService.GetInspections(dataId, cancellationToken);

                    if (rawData is null)
                    {
                        throw new InvalidOperationException(ApplicationConstants.External.SERVER_IS_NOT_RESPONDING);
                    }
                    else if (rawData == "[]")
                    {
                        return Result.Success;
                    }

                    await vehicleRepository.SaveInspection(request.RegistrationNumber, rawData, request.UserId);
                }

                return Result.Success;
            }
        }
    }
}

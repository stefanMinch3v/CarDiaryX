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
    public class UpdateVehicleDMRCommand : IRequest<Result>
    {
        public UpdateVehicleDMRCommand(string registrationNumber, string userId)
        {
            this.RegistrationNumber = registrationNumber;
            this.UserId = userId;
        }

        public string RegistrationNumber { get; }
        public string UserId { get; }

        internal class UpdateVehicleDMRCommandHanlder : IRequestHandler<UpdateVehicleDMRCommand, Result>
        {
            private readonly IServiceProvider serviceProvider;

            public UpdateVehicleDMRCommandHanlder(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            public async Task<Result> Handle(UpdateVehicleDMRCommand request, CancellationToken cancellationToken)
            {
                using var scope = this.serviceProvider.CreateScope();

                var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
                var vehicleHttpService = scope.ServiceProvider.GetRequiredService<IVehicleHttpService>();

                var existingDMR = await vehicleRepository.GetDMR(request.RegistrationNumber, cancellationToken);

                if (existingDMR is null || !DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, existingDMR.CreatedOn))
                {
                    var (tsId, _) = await vehicleRepository.GetDataAndTsIds(request.RegistrationNumber, cancellationToken);
                    var rootDMR = await vehicleHttpService.GetDMR(tsId, cancellationToken);

                    if (rootDMR is null)
                    {
                        throw new InvalidOperationException(ApplicationConstants.External.SERVER_IS_NOT_RESPONDING);
                    }
                    else if (string.IsNullOrEmpty(rootDMR.RawData))
                    {
                        throw new InvalidOperationException(ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER);
                    }

                    await vehicleRepository.SaveDMR(
                        request.RegistrationNumber,
                        rootDMR.GetNextGreenTaxDate,
                        rootDMR.GetNextInspectionDate,
                        rootDMR.RawData,
                        request.UserId);
                }

                return Result.Success;
            }
        }
    }
}

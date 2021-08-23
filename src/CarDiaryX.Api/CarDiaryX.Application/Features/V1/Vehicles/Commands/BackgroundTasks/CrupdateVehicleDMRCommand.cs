using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Domain.Integration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands.BackgroundTasks
{
    public class CrupdateVehicleDMRCommand : IRequest<Result>
    {
        public CrupdateVehicleDMRCommand(string registrationNumber, string userId, RootDMR rootDMR = null)
        {
            this.RegistrationNumber = registrationNumber;
            this.UserId = userId;
            this.RootDMR = rootDMR;
        }

        public string RegistrationNumber { get; }

        public string UserId { get; }

        public RootDMR RootDMR { get; }

        internal class CrupdateVehicleDMRCommandHanlder : IRequestHandler<CrupdateVehicleDMRCommand, Result>
        {
            private readonly IServiceProvider serviceProvider;

            public CrupdateVehicleDMRCommandHanlder(IServiceProvider serviceProvider)
                => this.serviceProvider = serviceProvider;

            public async Task<Result> Handle(CrupdateVehicleDMRCommand request, CancellationToken cancellationToken)
            {
                using var scope = this.serviceProvider.CreateScope();

                var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();

                if (request.RootDMR is not null)
                {
                    await vehicleRepository.UpdateDMR(
                        request.RegistrationNumber,
                        request.RootDMR?.GetNextGreenTaxDate,
                        request.RootDMR?.GetNextInspectionDate,
                        request.RootDMR?.RawData,
                        request.UserId);

                    return Result.Success;
                }

                var vehicleHttpService = scope.ServiceProvider.GetRequiredService<IVehicleHttpService>();
                var backgroundTaskQueue = scope.ServiceProvider.GetRequiredService<IBackgroundTaskQueue>();

                var existingDMR = await vehicleRepository.GetDMR(request.RegistrationNumber, cancellationToken);

                if (existingDMR is null)
                {
                    var rootDMR = await this.GetRootDMR(
                        request.RegistrationNumber,
                        request.UserId,
                        vehicleHttpService,
                        vehicleRepository,
                        backgroundTaskQueue,
                        cancellationToken);

                    await vehicleRepository.SaveDMR(
                        request.RegistrationNumber,
                        rootDMR.GetNextGreenTaxDate,
                        rootDMR.GetNextInspectionDate,
                        rootDMR.RawData,
                        request.UserId);
                }
                else if (!DateTimeHelper.AreDatesInTheSameMonth(DateTimeOffset.UtcNow, existingDMR.CreatedOn))
                {
                    var rootDMR = await this.GetRootDMR(
                        request.RegistrationNumber,
                        request.UserId,
                        vehicleHttpService,
                        vehicleRepository,
                        backgroundTaskQueue,
                        cancellationToken);

                    await vehicleRepository.UpdateDMR(
                        request.RegistrationNumber,
                        rootDMR.GetNextGreenTaxDate,
                        rootDMR.GetNextInspectionDate,
                        rootDMR.RawData,
                        request.UserId);
                }

                return Result.Success;
            }

            private async Task<RootDMR> GetRootDMR(
                string registrationNumber,
                string userId,
                IVehicleHttpService vehicleHttpService,
                IVehicleRepository vehicleRepository,
                IBackgroundTaskQueue backgroundTaskQueue,
                CancellationToken cancellationToken)
            {
                var (tsId, _, createdOn) = await vehicleRepository.GetParamsForExternalCall(registrationNumber, cancellationToken);

                if (tsId == 0)
                {
                    throw new InvalidOperationException(string.Format(ApplicationConstants.External.INVALID_TSID_DATAID_PROPERTIES, registrationNumber));
                }

                if (!DateTimeHelper.AreDatesInTheSameMonth(DateTimeOffset.UtcNow, createdOn))
                {
                    var rootInformation = await vehicleHttpService.GetInformation(registrationNumber, cancellationToken);

                    if (rootInformation is null)
                    {
                        throw new InvalidOperationException(ApplicationConstants.External.SERVER_IS_NOT_RESPONDING);
                    }
                    else if (rootInformation.Data is null)
                    {
                        throw new InvalidOperationException(ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER);
                    }

                    tsId = rootInformation.Data.TsId;

                    Task<IRequest<Result>> informationWorkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInformationCommand(registrationNumber, userId, rootInformation));

                    await backgroundTaskQueue.EnqueueWorkItem(informationWorkItem);
                }

                var rootDMR = await vehicleHttpService.GetDMR(tsId, cancellationToken);

                if (rootDMR is null)
                {
                    throw new InvalidOperationException(ApplicationConstants.External.SERVER_IS_NOT_RESPONDING);
                }
                else if (string.IsNullOrEmpty(rootDMR.RawData))
                {
                    throw new InvalidOperationException(ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER);
                }

                return rootDMR;
            }
        }
    }
}

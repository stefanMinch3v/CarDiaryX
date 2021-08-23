using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands.BackgroundTasks
{
    [Obsolete(ApplicationConstants.HALTED_FEATURES)]
    public class CrupdateVehicleInspectionCommand : IRequest<Result>
    {
        public CrupdateVehicleInspectionCommand(string registrationNumber, string userId, string inspectionsJsonData = null)
        {
            this.RegistrationNumber = registrationNumber;
            this.UserId = userId;
            this.InspectionsJsonData = inspectionsJsonData;
        }

        public string RegistrationNumber { get; }

        public string UserId { get; }

        public string InspectionsJsonData { get; }

        internal class CrupdateVehicleInspectionCommandHandler : IRequestHandler<CrupdateVehicleInspectionCommand, Result>
        {
            private readonly IServiceProvider serviceProvider;

            public CrupdateVehicleInspectionCommandHandler(IServiceProvider serviceProvider)
                => this.serviceProvider = serviceProvider;

            public async Task<Result> Handle(CrupdateVehicleInspectionCommand request, CancellationToken cancellationToken)
            {
                using var scope = this.serviceProvider.CreateScope();

                var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
                var vehicleHttpService = scope.ServiceProvider.GetRequiredService<IVehicleHttpService>();
                var backgroundTaskQueue = scope.ServiceProvider.GetRequiredService<IBackgroundTaskQueue>();

                if (!string.IsNullOrEmpty(request.InspectionsJsonData))
                {
                    await vehicleRepository.SaveInspection(request.RegistrationNumber, request.InspectionsJsonData, request.UserId);
                    return Result.Success;
                }

                var existingInspection = await vehicleRepository.GetInspection(request.RegistrationNumber, cancellationToken);

                if (existingInspection is null)
                {
                    var rawData = await this.GetInspectionsJson(
                        request.RegistrationNumber,
                        request.UserId,
                        vehicleHttpService,
                        vehicleRepository,
                        backgroundTaskQueue,
                        cancellationToken);

                    if (rawData == "[]")
                    {
                        return Result.Success;
                    }

                    await vehicleRepository.SaveInspection(request.RegistrationNumber, rawData, request.UserId);
                }
                else if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, existingInspection.CreatedOn))
                {
                    var rawData = await this.GetInspectionsJson(
                        request.RegistrationNumber,
                        request.UserId,
                        vehicleHttpService,
                        vehicleRepository,
                        backgroundTaskQueue,
                        cancellationToken);

                    if (rawData == "[]")
                    {
                        return Result.Success;
                    }

                    await vehicleRepository.UpdateInspection(request.RegistrationNumber, rawData, request.UserId);
                }

                return Result.Success;
            }

            private async Task<string> GetInspectionsJson(
                string registrationNumber,
                string userId,
                IVehicleHttpService vehicleHttpService,
                IVehicleRepository vehicleRepository,
                IBackgroundTaskQueue backgroundTaskQueue,
                CancellationToken cancellationToken)
            {
                var (_, dataId, createdOn) = await vehicleRepository.GetParamsForExternalCall(registrationNumber, cancellationToken);

                if (dataId == 0)
                {
                    throw new InvalidOperationException(string.Format(ApplicationConstants.External.INVALID_TSID_DATAID_PROPERTIES, registrationNumber));
                }

                if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, createdOn))
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

                    dataId = rootInformation.Data.Id;

                    Task<IRequest<Result>> updateInformationWorkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInformationCommand(registrationNumber, userId, rootInformation));

                    await backgroundTaskQueue.EnqueueWorkItem(updateInformationWorkItem);
                }

                // slow request 5-8 seconds
                var rawData = await vehicleHttpService.GetInspections(dataId, cancellationToken);

                if (rawData is null)
                {
                    throw new InvalidOperationException(ApplicationConstants.External.SERVER_IS_NOT_RESPONDING);
                }

                return rawData;
            }
        }
    }
}

using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Vehicles.Commands.BackgroundTasks;
using CarDiaryX.Application.Features.V1.Vehicles.OutputModels;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    [Obsolete(ApplicationConstants.HALTED_FEATURES)]
    public class GetVehicleInspectionQuery : IRequest<Result<VehicleSharedOutputModel>>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleInspectionQueryHandler : IRequestHandler<GetVehicleInspectionQuery, Result<VehicleSharedOutputModel>>
        {
            private readonly IVehicleRepository vehicleRepository;
            private readonly IPermissionRepository permissionRepository;
            private readonly IVehicleHttpService vehicleHttpService;
            private readonly ICurrentUser currentUser;
            private readonly IBackgroundTaskQueue backgroundTaskQueue;
            private readonly IRegistrationNumberRepository registrationNumberRepository;

            public GetVehicleInspectionQueryHandler(
                IVehicleRepository vehicleRepository,
                IPermissionRepository permissionRepository,
                IVehicleHttpService vehicleHttpService,
                ICurrentUser currentUser,
                IBackgroundTaskQueue backgroundTaskQueue,
                IRegistrationNumberRepository registrationNumberRepository)
            {
                this.vehicleRepository = vehicleRepository;
                this.permissionRepository = permissionRepository;
                this.vehicleHttpService = vehicleHttpService;
                this.currentUser = currentUser;
                this.backgroundTaskQueue = backgroundTaskQueue;
                this.registrationNumberRepository = registrationNumberRepository;
            }

            public async Task<Result<VehicleSharedOutputModel>> Handle(GetVehicleInspectionQuery request, CancellationToken cancellationToken)
            {
                var permission = await this.permissionRepository.GetByUser(cancellationToken);

                if (permission?.PermissionType != PermissionType.Premium
                    || permission?.PermissionType != PermissionType.Professional)
                {
                    return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.Permissions.ACCOUNT_HAS_NO_PERMISSIONS });
                }

                var hasUserNumber = await this.registrationNumberRepository.DoesBelongToUser(request.RegistrationNumber, cancellationToken);

                if (!hasUserNumber)
                {
                    var errors = new[] { string.Format(ApplicationConstants.Vehicles.INVALID_VEHICLE_NOT_BELONGING_TO_USER_GARAGE, request.RegistrationNumber) };
                    return Result<VehicleSharedOutputModel>.Failure(errors);
                }

                var jsonData = string.Empty;
                var inspection = await this.vehicleRepository.GetInspection(request.RegistrationNumber, cancellationToken);
                jsonData = inspection?.JsonData;

                if (inspection is null)
                {
                    var (_, dataId, createdOn) = await this.vehicleRepository.GetParamsForExternalCall(request.RegistrationNumber, cancellationToken);

                    if (dataId == 0)
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.Vehicles.DELETED_VEHICLE_FROM_GARAGE });
                    }

                    if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.Now, createdOn))
                    {
                        var (jsonInspections, errors) = await this.ExecuteWhenDatesAreNotInSameFieldFlow(request.RegistrationNumber, cancellationToken);

                        if (errors.Length > 0)
                        {
                            return Result<VehicleSharedOutputModel>.Failure(errors);
                        }

                        return new VehicleSharedOutputModel
                        {
                            JsonData = jsonInspections
                        };
                    }

                    // slow request 5-8 seconds
                    jsonData = await this.vehicleHttpService.GetInspections(dataId, cancellationToken);

                    if (string.IsNullOrEmpty(jsonData))
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING });
                    }
                    else if (jsonData == "[]")
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER });
                    }

                    Task<IRequest<Result>> saveInspectionWorkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInspectionCommand(request.RegistrationNumber, this.currentUser.UserId, jsonData));

                    await this.backgroundTaskQueue.EnqueueWorkItem(saveInspectionWorkItem);
                }
                else if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, inspection.CreatedOn))
                {
                    var (jsonInspections, errors) = await this.ExecuteWhenDatesAreNotInSameFieldFlow(request.RegistrationNumber, cancellationToken);

                    if (errors.Length > 0)
                    {
                        return Result<VehicleSharedOutputModel>.Failure(errors);
                    }

                    jsonData = jsonInspections;
                }

                return new VehicleSharedOutputModel
                {
                    JsonData = jsonData
                };
            }

            private async Task<(string JsonData, string[] Errors)> ExecuteWhenDatesAreNotInSameFieldFlow(string registrationNumber, CancellationToken cancellationToken)
            {
                var jsonData = string.Empty;
                var errors = Array.Empty<string>();

                var rootInformation = await this.vehicleHttpService.GetInformation(registrationNumber, cancellationToken);

                if (rootInformation is null)
                {
                    errors = new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING };
                }
                else if (rootInformation.Data is null)
                {
                    errors = new[] { ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER };
                }

                if (errors.Length > 0)
                {
                    return (jsonData, errors);
                }

                // slow request 5-8 seconds
                jsonData = await this.vehicleHttpService.GetInspections(rootInformation.Data.Id, cancellationToken);

                Task<IRequest<Result>> updateInformationworkItem(CancellationToken token)
                    => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInformationCommand(registrationNumber, this.currentUser.UserId, rootInformation));

                await this.backgroundTaskQueue.EnqueueWorkItem(updateInformationworkItem);

                Task<IRequest<Result>> saveInspectionworkItem(CancellationToken token)
                    => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInspectionCommand(registrationNumber, this.currentUser.UserId, jsonData));

                await this.backgroundTaskQueue.EnqueueWorkItem(saveInspectionworkItem);

                return (jsonData, errors);
            }
        }
    }
}

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
    public class GetVehicleDMRQuery : IRequest<Result<VehicleSharedOutputModel>>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleDMRQueryHandler : IRequestHandler<GetVehicleDMRQuery, Result<VehicleSharedOutputModel>>
        {
            private readonly IVehicleRepository vehicleRepository;
            private readonly IPermissionRepository permissionRepository;
            private readonly ICurrentUser currentUser;
            private readonly IBackgroundTaskQueue backgroundTaskQueue;
            private readonly IVehicleHttpService vehicleHttpService;

            public GetVehicleDMRQueryHandler(
                IVehicleRepository vehicleRepository,
                IPermissionRepository permissionRepository,
                ICurrentUser currentUser,
                IBackgroundTaskQueue backgroundTaskQueue,
                IVehicleHttpService vehicleHttpService)
            {
                this.vehicleRepository = vehicleRepository;
                this.permissionRepository = permissionRepository;
                this.currentUser = currentUser;
                this.backgroundTaskQueue = backgroundTaskQueue;
                this.vehicleHttpService = vehicleHttpService;
            }

            public async Task<Result<VehicleSharedOutputModel>> Handle(GetVehicleDMRQuery request, CancellationToken cancellationToken)
            {
                var permission = await this.permissionRepository.GetByUser(cancellationToken);

                if (permission?.PermissionType != PermissionType.Premium
                    || permission?.PermissionType != PermissionType.Professional)
                {
                    return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.Permissions.ACCOUNT_HAS_NO_PERMISSIONS });
                }

                var jsonData = string.Empty;
                var dmr = await this.vehicleRepository.GetDMR(request.RegistrationNumber, cancellationToken);
                jsonData = dmr?.JsonData;

                if (dmr is null)
                {
                    var (tsId, _, _) = await this.vehicleRepository.GetParamsForExternalCall(request.RegistrationNumber, cancellationToken);

                    if (tsId == 0)
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.Vehicles.DELETED_VEHICLE_FROM_GARAGE });
                    }

                    var rootDMR = await this.vehicleHttpService.GetDMR(tsId, cancellationToken);

                    if (rootDMR is null)
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING });
                    }
                    else if (string.IsNullOrEmpty(rootDMR.RawData))
                    {
                        return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER });
                    }

                    jsonData = rootDMR.RawData;

                    Task<IRequest<Result>> saveDMRworkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleDMRCommand(request.RegistrationNumber, this.currentUser.UserId, rootDMR));

                    await this.backgroundTaskQueue.EnqueueWorkItem(saveDMRworkItem);
                }
                else if (!DateTimeHelper.AreDatesInTheSameMonth(DateTimeOffset.UtcNow, dmr.CreatedOn))
                {
                    Task<IRequest<Result>> updateDMRworkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleDMRCommand(request.RegistrationNumber, this.currentUser.UserId));

                    await this.backgroundTaskQueue.EnqueueWorkItem(updateDMRworkItem);
                }

                return new VehicleSharedOutputModel
                {
                    JsonData = jsonData
                };
            }
        }
    }
}

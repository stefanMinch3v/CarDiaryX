using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Vehicles.Commands.BackgroundTasks;
using CarDiaryX.Application.Features.V1.Vehicles.OutputModels;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetVehicleInformationQuery : IRequest<Result<VehicleSharedOutputModel>>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleInformationQueryHandler : IRequestHandler<GetVehicleInformationQuery, Result<VehicleSharedOutputModel>>
        {
            private readonly IVehicleRepository vehicleRepository;
            private readonly IBackgroundTaskQueue backgroundTaskQueue;
            private readonly ICurrentUser currentUser;
            private readonly IRegistrationNumberRepository registrationNumberRepository;

            public GetVehicleInformationQueryHandler(
                IVehicleRepository vehicleRepository, 
                IBackgroundTaskQueue backgroundTaskQueue,
                ICurrentUser currentUser,
                IRegistrationNumberRepository registrationNumberRepository)
            {
                this.vehicleRepository = vehicleRepository;
                this.backgroundTaskQueue = backgroundTaskQueue;
                this.currentUser = currentUser;
                this.registrationNumberRepository = registrationNumberRepository;
            }

            public async Task<Result<VehicleSharedOutputModel>> Handle(GetVehicleInformationQuery request, CancellationToken cancellationToken)
            {
                var hasUserNumber = await this.registrationNumberRepository.DoesBelongToUser(request.RegistrationNumber, cancellationToken);

                if (!hasUserNumber)
                {
                    var errors = new[] { string.Format(ApplicationConstants.Vehicles.INVALID_VEHICLE_NOT_BELONGING_TO_USER_GARAGE, request.RegistrationNumber) };
                    return Result<VehicleSharedOutputModel>.Failure(errors);
                }

                var information = await this.vehicleRepository.GetInformation(
                    request.RegistrationNumber,
                    cancellationToken);

                if (information is null)
                {
                    return Result<VehicleSharedOutputModel>.Failure(new[] { ApplicationConstants.Vehicles.DELETED_VEHICLE_FROM_GARAGE });
                }

                if (!DateTimeHelper.AreDatesInTheSameMonth(DateTimeOffset.UtcNow, information.CreatedOn))
                {
                    Task<IRequest<Result>> informationWorkItem(CancellationToken token)
                        => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInformationCommand(request.RegistrationNumber, this.currentUser.UserId));

                    await this.backgroundTaskQueue.EnqueueWorkItem(informationWorkItem);
                }

                return new VehicleSharedOutputModel
                {
                    JsonData = information.JsonData
                };
            }
        }
    }
}

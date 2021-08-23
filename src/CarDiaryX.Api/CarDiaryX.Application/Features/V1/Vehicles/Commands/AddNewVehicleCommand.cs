using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.BackgroundServices;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Vehicles.Commands.BackgroundTasks;
using CarDiaryX.Domain.Integration;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Commands
{
    public class AddNewVehicleCommand : IRequest<Result>
    {
        public string RegistrationNumber { get; set; }

        internal class AddNewVehicleCommandCommandHandler : IRequestHandler<AddNewVehicleCommand, Result>
        {
            private readonly IVehicleHttpService vehicleHttpService;
            private readonly IVehicleRepository vehicleRepository;
            private readonly IRegistrationNumberRepository registrationNumberRepository;
            private readonly IPermissionRepository permissionRepository;
            private readonly IBackgroundTaskQueue backgroundTaskQueue;
            private readonly ICurrentUser currentUser;

            public AddNewVehicleCommandCommandHandler(
                IVehicleHttpService vehicleHttpService,
                IVehicleRepository vehicleRepository,
                IRegistrationNumberRepository registrationNumberRepository,
                IPermissionRepository permissionRepository,
                IBackgroundTaskQueue backgroundTaskQueue,
                ICurrentUser currentUser)
            {
                this.vehicleHttpService = vehicleHttpService;
                this.vehicleRepository = vehicleRepository;
                this.registrationNumberRepository = registrationNumberRepository;
                this.permissionRepository = permissionRepository;
                this.backgroundTaskQueue = backgroundTaskQueue;
                this.currentUser = currentUser;
            }

            public async Task<Result> Handle(AddNewVehicleCommand request, CancellationToken cancellationToken)
            {
                var permission = await this.permissionRepository.GetByUser(cancellationToken);

                var existingRegistrationNumber = await this.registrationNumberRepository.Get(request.RegistrationNumber, cancellationToken);
                if (existingRegistrationNumber is not null)
                {
                    await this.registrationNumberRepository.AddToUser(existingRegistrationNumber.Id);

                    if (PermissionsHelper.IsPaidUser(permission))
                    {
                        await this.ExecuteBackgroundTasksForPaidUsers(request.RegistrationNumber, this.currentUser.UserId);
                    }

                    return Result.Success;
                }

                var vehicleRootInfo = await this.vehicleHttpService.GetInformation(request.RegistrationNumber, cancellationToken);
                if (vehicleRootInfo is null)
                {
                    var errors = new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING };
                    return Result.Failure(errors);
                }
                else if (vehicleRootInfo.Data is null)
                {
                    var errors = new[] { ApplicationConstants.External.NO_RESULTS_FOUND_ON_THE_SERVER };
                    return Result.Failure(errors);
                }

                await this.vehicleRepository.SaveInformation(request.RegistrationNumber, vehicleRootInfo);

                if (PermissionsHelper.IsPaidUser(permission))
                {
                    await this.ExecuteBackgroundTasksForPaidUsers(request.RegistrationNumber, this.currentUser.UserId);
                }

                var shortDescription = this.BuildShortDescription(vehicleRootInfo);
                var savedId = await this.registrationNumberRepository.Save(request.RegistrationNumber, shortDescription, vehicleRootInfo.Data.CarType);
                await this.registrationNumberRepository.AddToUser(savedId);

                return Result.Success;
            }

            private string BuildShortDescription(RootInformation rootInformation)
            {
                if (rootInformation is null || rootInformation.Data is null)
                {
                    return null;
                }

                const string identifier = "^";
                var sb = new StringBuilder();

                sb.Append(rootInformation.Data.Brand);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.Model);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.Version ??= string.Empty);
                sb.Append(identifier);
                sb.Append(rootInformation.Data.ModelYear?.ToString());
                sb.Append(identifier);
                sb.Append(rootInformation.Data.FuelType ??= string.Empty);

                return sb.ToString().TrimEnd();
            }

            private async Task ExecuteBackgroundTasksForPaidUsers(string registrationNumber, string userId)
            {
                Task<IRequest<Result>> saveTaskDMR(CancellationToken token)
                    => Task.FromResult<IRequest<Result>>(new CrupdateVehicleDMRCommand(registrationNumber, userId));

                await this.backgroundTaskQueue.EnqueueWorkItem(saveTaskDMR);

                // Upcoming feature
                //Task<IRequest<Result>> saveTaskInspections(CancellationToken token)
                //    => Task.FromResult<IRequest<Result>>(new CrupdateVehicleInspectionCommand(registrationNumber, userId));
                //await this.backgroundTaskQueue.EnqueueWorkItem(saveTaskInspections);
            }
        }
    }
}

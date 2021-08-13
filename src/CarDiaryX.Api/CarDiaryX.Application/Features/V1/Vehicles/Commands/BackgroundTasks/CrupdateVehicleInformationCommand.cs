using CarDiaryX.Application.Common;
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
    public class CrupdateVehicleInformationCommand : IRequest<Result>
    {
        public CrupdateVehicleInformationCommand(string registrationNumber, string userId, RootInformation rootInformation = null)
        {
            this.RegistrationNumber = registrationNumber;
            this.UserId = userId;
            this.RootInformation = rootInformation;
        }

        public string RegistrationNumber { get; }

        public string UserId { get; }

        public RootInformation RootInformation { get; }

        internal class CrupdateVehicleInformationCommandHandler : IRequestHandler<CrupdateVehicleInformationCommand, Result>
        {
            private readonly IServiceProvider serviceProvider;

            public CrupdateVehicleInformationCommandHandler(IServiceProvider serviceProvider) 
                => this.serviceProvider = serviceProvider;

            public async Task<Result> Handle(CrupdateVehicleInformationCommand request, CancellationToken cancellationToken)
            {
                using var scope = this.serviceProvider.CreateScope();

                var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
                var vehicleHttpService = scope.ServiceProvider.GetRequiredService<IVehicleHttpService>();

                if (request.RootInformation is not null)
                {
                    await vehicleRepository.UpdateInformation(request.RegistrationNumber, request.RootInformation, request.UserId);
                    return Result.Success;
                }

                var existingInformation = await vehicleRepository.GetInformation(request.RegistrationNumber, cancellationToken);

                if (existingInformation is null)
                {
                    var rootInformation = await this.GetRootInformation(request.RegistrationNumber, vehicleHttpService, cancellationToken);

                    await vehicleRepository.SaveInformation(request.RegistrationNumber, rootInformation, request.UserId);
                }
                else if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, existingInformation.CreatedOn))
                {
                    var rootInformation = await this.GetRootInformation(request.RegistrationNumber, vehicleHttpService, cancellationToken);

                    await vehicleRepository.UpdateInformation(request.RegistrationNumber, rootInformation, request.UserId);
                }

                return Result.Success;
            }

            private async Task<RootInformation> GetRootInformation(string registrationNumber, IVehicleHttpService vehicleHttpService, CancellationToken cancellationToken)
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

                return rootInformation;
            }
        }
    }
}

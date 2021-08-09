using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetVehicleInspectionQuery : IRequest<object>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleInspectionQueryHandler : IRequestHandler<GetVehicleInspectionQuery, object>
        {
            private readonly IVehicleRepository vehicleRepository;
            private readonly IPermissionRepository permissionRepository;
            private readonly IVehicleHttpService vehicleHttpService;

            public GetVehicleInspectionQueryHandler(
                IVehicleRepository vehicleRepository, 
                IPermissionRepository permissionRepository,
                IVehicleHttpService vehicleHttpService)
            {
                this.vehicleRepository = vehicleRepository;
                this.permissionRepository = permissionRepository;
                this.vehicleHttpService = vehicleHttpService;
            }

            public async Task<object> Handle(GetVehicleInspectionQuery request, CancellationToken cancellationToken)
            {
                var permission = await this.permissionRepository.GetByUser(cancellationToken);

                if (permission?.PermissionType != PermissionType.Premium 
                    || permission?.PermissionType != PermissionType.Professional)
                {
                    return Result.Failure(new[] { "" });
                }

                var existingInspection = await this.vehicleRepository.GetInspection(request.RegistrationNumber, cancellationToken);

                if (existingInspection is not null)
                {
                    if (DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, existingInspection.CreatedOn))
                    {
                        return existingInspection;
                    }
                    else
                    {
                        // TODO run background task to update
                    }
                }

                var (tsId, dataId) = await this.vehicleRepository.GetDataAndTsIds(request.RegistrationNumber, cancellationToken);

                if (tsId == 0 || dataId == 0)
                {
                    return Result.Failure(new[] { "" });
                }

                // slow request 5-8 seconds
                var vehicleInspectionsContent = await this.vehicleHttpService.GetInspections(dataId, cancellationToken); 

                if (vehicleInspectionsContent is null)
                {
                    //var errors = new[] { ApplicationConstants.External.SERVER_IS_NOT_RESPONDING };
                    return Result.Failure(new[] { "" });
                }
                // TODO: parse data: nextGreenTaxDate, nextInspectionDate
                //await this.vehicleRepository.SaveDMR(
                //    request.RegistrationNumber,
                //    DateTime.Now, //nextGreenTaxDate
                //    DateTime.Now, //nextInspectionDate
                //    vehicleDMRRawContent);

                return vehicleInspectionsContent;
            }
        }
    }
}

using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetVehicleDMRQuery : IRequest<Result<object>>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleDMRQueryHandler : IRequestHandler<GetVehicleDMRQuery, Result<object>>
        {
            private readonly IVehicleRepository vehicleRepository;
            private readonly IPermissionRepository permissionRepository;

            public GetVehicleDMRQueryHandler(
                IVehicleRepository vehicleRepository, 
                IPermissionRepository permissionRepository)
            {
                this.vehicleRepository = vehicleRepository;
                this.permissionRepository = permissionRepository;
            }

            public async Task<Result<object>> Handle(GetVehicleDMRQuery request, CancellationToken cancellationToken)
            {
                var permission = await this.permissionRepository.GetByUser(cancellationToken);

                if (permission?.PermissionType != PermissionType.Premium
                    || permission?.PermissionType != PermissionType.Professional)
                {
                    return Result.Failure(new[] { "TODO" });
                }

                var dmr = await this.vehicleRepository.GetDMR(request.RegistrationNumber, cancellationToken);

                if (dmr is null)
                {
                    // make request to external
                }
                else if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, dmr.CreatedOn))
                {
                    // TODO background task to update
                }

                return dmr.JsonData;
            }
        }
    }
}

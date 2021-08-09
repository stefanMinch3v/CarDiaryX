using CarDiaryX.Application.Common.Helpers;
using CarDiaryX.Application.Features.V1.Vehicles.OutputModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles.Queries
{
    public class GetVehicleInformationQuery : IRequest<VehicleInformationOutputModel>
    {
        public string RegistrationNumber { get; set; }

        internal class GetVehicleInformationQueryHandler : IRequestHandler<GetVehicleInformationQuery, VehicleInformationOutputModel>
        {
            private readonly IVehicleRepository vehicleRepository;

            public GetVehicleInformationQueryHandler(IVehicleRepository vehicleRepository)
            {
                this.vehicleRepository = vehicleRepository;
            }

            // TODO: Check if the created on is in the same week as datetime now
            public async Task<VehicleInformationOutputModel> Handle(GetVehicleInformationQuery request, CancellationToken cancellationToken)
            {
                var information = await this.vehicleRepository.GetInformation(
                    request.RegistrationNumber,
                    cancellationToken);

                if (!DateTimeHelper.AreDatesInTheSameWeek(DateTimeOffset.UtcNow, information.CreatedOn))
                {
                    // TODO Run background task to fetch and update the database
                }

                return new VehicleInformationOutputModel
                {
                    JsonDataInformation = information.JsonData
                };
            }
        }
    }
}

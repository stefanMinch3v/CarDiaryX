using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.VehicleServices.OutputModels;
using CarDiaryX.Domain.VehicleServices;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Queries
{
    public class GetVehicleServicesQuery : IRequest<Result<VehicleServiceOutputModel>>
    {
        public int VehicleServicesId { get; set; }

        internal class GetVehicleServicesQueryHandler : IRequestHandler<GetVehicleServicesQuery, Result<VehicleServiceOutputModel>>
        {
            private readonly IVehicleServicesRepository vehicleServicesRepository;

            public GetVehicleServicesQueryHandler(IVehicleServicesRepository vehicleServicesRepository) 
                => this.vehicleServicesRepository = vehicleServicesRepository;

            public async Task<Result<VehicleServiceOutputModel>> Handle(GetVehicleServicesQuery request, CancellationToken cancellationToken)
            {
                var vehicleService = await this.vehicleServicesRepository.GetShallow(request.VehicleServicesId, cancellationToken);
                return this.MapTo(vehicleService);
            }

            private ReviewOutputModel MapTo(Review review)
            {
                if (review is null)
                {
                    return null;
                }

                return new ReviewOutputModel
                {
                    Name = review.Name,
                    CreatedOn = review.CreatedOn,
                    Prices = (int)review.Prices,
                    Ratings = (int)review.Ratings,
                    CreatedByUser = review.CreatedBy // usually this is guid id
                };
            }

            private VehicleServiceOutputModel MapTo(VehicleService vehicleService)
            {
                if (vehicleService is null)
                {
                    return null;
                }

                return new VehicleServiceOutputModel
                {
                    Description = vehicleService.Description,
                    Name = vehicleService.Name,
                    Reviews = vehicleService.Reviews.Select(this.MapTo).ToArray()
                };
            }
        }
    }
}

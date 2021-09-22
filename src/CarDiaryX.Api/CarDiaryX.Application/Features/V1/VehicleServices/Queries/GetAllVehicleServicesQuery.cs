using CarDiaryX.Application.Common;
using CarDiaryX.Application.Features.V1.VehicleServices.OutputModels;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.VehicleServices;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Queries
{
    public class GetAllVehicleServicesQuery : IRequest<Result<PagingModel<VehicleServiceListingOutputModel>>>
    {
        public string SearchText { get; set; }
        public int Page { get; set; }

        internal class GetAllVehicleServicesQueryHandler : IRequestHandler<GetAllVehicleServicesQuery, Result<PagingModel<VehicleServiceListingOutputModel>>>
        {
            private readonly IVehicleServicesRepository vehicleServicesRepository;

            public GetAllVehicleServicesQueryHandler(IVehicleServicesRepository vehicleServicesRepository)
            {
                this.vehicleServicesRepository = vehicleServicesRepository;
            }

            public async Task<Result<PagingModel<VehicleServiceListingOutputModel>>> Handle(GetAllVehicleServicesQuery request, CancellationToken cancellationToken)
            {
                var result = await this.vehicleServicesRepository.GetAllShallow(cancellationToken, request.SearchText, request.Page);
                return new PagingModel<VehicleServiceListingOutputModel>(result.Collection.Select(this.MapTo).ToArray(), result.TotalCount);
            }

            private VehicleServiceListingOutputModel MapTo(VehicleService vehicleService)
            {
                if (vehicleService is null)
                {
                    return null;
                }

                return new VehicleServiceListingOutputModel
                {
                    Id = vehicleService.Id,
                    Address = vehicleService.Address,
                    Name = vehicleService.Name,
                    Description = vehicleService.Description,
                    AverageReviewPrices = vehicleService.GetAverageReviewPrices(),
                    AverageReviewRatings = vehicleService.GetAverageReviewRatings(),
                    NumberOfReviews = vehicleService.GetNumOfReviews()
                };
            }
        }
    }
}

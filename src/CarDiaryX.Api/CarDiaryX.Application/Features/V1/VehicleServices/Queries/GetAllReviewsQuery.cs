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
    public class GetAllReviewsQuery : IRequest<Result<PagingModel<ReviewOutputModel>>>
    {
        public int Page { get; set; }

        internal class GetAllReviewsQueryHanlder : IRequestHandler<GetAllReviewsQuery, Result<PagingModel<ReviewOutputModel>>>
        {
            private readonly IVehicleServicesRepository vehicleServicesRepository;

            public GetAllReviewsQueryHanlder(IVehicleServicesRepository vehicleServicesRepository) 
                => this.vehicleServicesRepository = vehicleServicesRepository;

            public async Task<Result<PagingModel<ReviewOutputModel>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
            {
                var result = await this.vehicleServicesRepository.GetAllReviews(cancellationToken, request.Page);
                return new PagingModel<ReviewOutputModel>(result.Collection.Select(this.MapTo).ToArray(), result.TotalCount);
            }

            private ReviewOutputModel MapTo(Review review)
            {
                if (review is null)
                {
                    return null;
                }

                return new ReviewOutputModel
                {
                    CreatedByUser = review.CreatedBy, // usually this is guid id
                    CreatedOn = review.CreatedOn,
                    Name = review.Name,
                    Prices = (int)review.Prices,
                    Ratings = (int)review.Ratings
                };
            }
        }
    }
}

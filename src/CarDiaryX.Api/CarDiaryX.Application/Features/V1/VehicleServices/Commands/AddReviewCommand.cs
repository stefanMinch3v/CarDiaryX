using CarDiaryX.Application.Common;
using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Domain.VehicleServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.VehicleServices.Commands
{
    public class AddReviewCommand : IRequest<Result>
    {
        public string Name { get; set; }

        public int Ratings { get; set; }

        public int Prices { get; set; }

        public int VehicleServiceId { get; set; }

        internal class AddReviewCommandHanlder : IRequestHandler<AddReviewCommand, Result>
        {
            private readonly IVehicleServicesRepository vehicleServicesRepository;

            public AddReviewCommandHanlder(IVehicleServicesRepository vehicleServicesRepository) 
                => this.vehicleServicesRepository = vehicleServicesRepository;

            public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
            {
                var success = await this.vehicleServicesRepository.AddReview(request.Name, request.VehicleServiceId, (Ratings)request.Ratings, (Prices)request.Prices);

                if (!success)
                {
                    return Result.Failure(new[] { ApplicationConstants.VehicleServices.INVALID_REVIEW_VEHICLE_SERVICE_TO_ATTACH });
                }

                return Result.Success;
            }
        }
    }
}

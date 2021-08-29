using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Trips.OutputModels;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Queries
{
    public class GetAllTripsQuery : IRequest<Result<IReadOnlyCollection<TripListOutputModel>>>
    {
        internal class GetAllTripsQueryHandler : IRequestHandler<GetAllTripsQuery, Result<IReadOnlyCollection<TripListOutputModel>>>
        {
            private readonly ITripRepository tripRepository;
            private readonly ICurrentUser currentUser;

            public GetAllTripsQueryHandler(ITripRepository tripRepository, ICurrentUser currentUser)
            {
                this.tripRepository = tripRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result<IReadOnlyCollection<TripListOutputModel>>> Handle(GetAllTripsQuery request, CancellationToken cancellationToken)
            {
                var trips = await this.tripRepository.GetAll(this.currentUser.UserId, cancellationToken);
                return trips
                    .Select(this.MapTo)
                    .ToArray();
            }

            private TripListOutputModel MapTo(Trip trip)
            {
                if (trip is null)
                {
                    return null;
                }

                return new TripListOutputModel
                {
                    Id = trip.Id,
                    RegistrationNumber = trip.RegistrationNumber,
                    DepartureDate = trip.DepartureDate,
                    ArrivalDate = trip.ArrivalDate,
                    Distance = trip.Distance,
                    Cost = trip.Cost,
                    DepartureAddress = trip.DepartureAddress,
                    ArrivalAddress = trip.ArrivalAddress
                };
            }
        }
    }
}

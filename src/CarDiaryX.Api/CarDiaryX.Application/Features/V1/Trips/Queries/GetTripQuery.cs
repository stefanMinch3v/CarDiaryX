using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Application.Features.V1.Trips.OutputModels;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Queries
{
    public class GetTripQuery : IRequest<Result<TripDetailsOutputModel>>
    {
        public int Id { get; set; }

        internal class GetTripQueryHandler : IRequestHandler<GetTripQuery, Result<TripDetailsOutputModel>>
        {
            private readonly ITripRepository tripRepository;
            private readonly ICurrentUser currentUser;

            public GetTripQueryHandler(ITripRepository tripRepository, ICurrentUser currentUser)
            {
                this.tripRepository = tripRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result<TripDetailsOutputModel>> Handle(GetTripQuery request, CancellationToken cancellationToken)
            {
                var trip = await this.tripRepository.Get(request.Id, this.currentUser.UserId, cancellationToken);
                return this.MapTo(trip);
            }

            private TripDetailsOutputModel MapTo(Trip trip)
            {
                if (trip is null)
                {
                    return null;
                }

                return new TripDetailsOutputModel
                {
                    Id = trip.Id,
                    RegistrationNumber = trip.RegistrationNumber,
                    DepartureDate = trip.DepartureDate,
                    ArrivalDate = trip.ArrivalDate,
                    Distance = trip.Distance,
                    Cost = trip.Cost,
                    Note = trip.Note,
                    DepartureAddress = new AddressInputModel
                    {
                        Name = trip.DepartureAddress,
                        X = trip.DepartureAddressX,
                        Y = trip.DepartureAddressY
                    },
                    ArrivalAddress = new AddressInputModel
                    {
                        Name = trip.ArrivalAddress,
                        X = trip.ArrivalAddressX,
                        Y = trip.ArrivalAddressY
                    },
                    CreatedOn = trip.CreatedOn,
                    ModifiedOn = trip.ModifiedOn
                };
            }
        }
    }
}

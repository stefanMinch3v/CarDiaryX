using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Application.Features.V1.Trips.OutputModels;
using CarDiaryX.Domain.Vehicles;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Queries
{
    public class GetAllTripsQuery : IRequest<Result<TripWrapperOutputModel>>
    {
        public int Page { get; set; }

        public string RegistrationNumber { get; set; }

        internal class GetAllTripsQueryHandler : IRequestHandler<GetAllTripsQuery, Result<TripWrapperOutputModel>>
        {
            private readonly ITripRepository tripRepository;
            private readonly ICurrentUser currentUser;

            public GetAllTripsQueryHandler(ITripRepository tripRepository, ICurrentUser currentUser)
            {
                this.tripRepository = tripRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result<TripWrapperOutputModel>> Handle(GetAllTripsQuery request, CancellationToken cancellationToken)
            {
                var (trips, totalCount) = await this.tripRepository.GetAll(this.currentUser.UserId, cancellationToken, request.RegistrationNumber, request.Page);

                return new TripWrapperOutputModel
                {
                    TotalCount = totalCount,
                    Trips = trips
                        .Select(this.MapTo)
                        .ToArray()
                };
            }

            private TripOutputModel MapTo(Trip trip)
            {
                if (trip is null)
                {
                    return null;
                }

                return new TripOutputModel
                {
                    Id = trip.Id,
                    RegistrationNumber = trip.RegistrationNumber,
                    DepartureDate = trip.DepartureDate,
                    ArrivalDate = trip.ArrivalDate,
                    Distance = trip.Distance,
                    Cost = trip.Cost,
                    DepartureAddress = new AddressInputModel
                    {
                        Name = trip.DepartureAddress,
                        X = trip.DepartureAddressX,
                        Y = trip.DepartureAddressY
                    },
                    ArrivalAddress = new AddressInputModel
                    {
                        Name = trip.ArrivalAddress,
                        Y = trip.ArrivalAddressY,
                        X = trip.ArrivalAddressX
                    },
                    Note = trip.Note,
                };
            }
        }
    }
}

using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Commands
{
    public class UpdateTripCommand : IRequest<Result>
    {
        public int Id { get; set; }

        public TripInputModel Trip { get; set; }

        internal class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, Result>
        {
            private readonly ITripRepository tripRepository;
            private readonly ICurrentUser currentUser;

            public UpdateTripCommandHandler(ITripRepository tripRepository, ICurrentUser currentUser)
            {
                this.tripRepository = tripRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
            {
                await this.tripRepository.Update(
                    request.Id,
                    this.currentUser.UserId,
                    request.Trip.RegistrationNumber,
                    request.Trip.DepartureDate,
                    request.Trip.ArrivalDate,
                    request.Trip.DepartureAddress,
                    request.Trip.ArrivalAddress,
                    request.Trip.Distance,
                    request.Trip.Cost,
                    request.Trip.Note);
                return Result.Success;
            }
        }
    }
}

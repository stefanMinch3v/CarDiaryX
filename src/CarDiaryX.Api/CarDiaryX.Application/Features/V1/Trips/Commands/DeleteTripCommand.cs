using CarDiaryX.Application.Common;
using CarDiaryX.Application.Contracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips.Commands
{
    public class DeleteTripCommand : IRequest<Result>
    {
        public int Id { get; set; }

        internal class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, Result>
        {
            private readonly ITripRepository tripRepository;
            private readonly ICurrentUser currentUser;

            public DeleteTripCommandHandler(ITripRepository tripRepository, ICurrentUser currentUser)
            {
                this.tripRepository = tripRepository;
                this.currentUser = currentUser;
            }

            public async Task<Result> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
            {
                await this.tripRepository.Delete(request.Id, this.currentUser.UserId);
                return Result.Success;
            }
        }
    }
}

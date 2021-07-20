using CarDiaryX.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IRegistrationNumberRepository
    {
        Task AddToUser(Guid registrationNumberId, string userId);
        Task<RegistrationNumber> Get(string registrationNumber);
        Task<IReadOnlyCollection<RegistrationNumber>> GetByUser(string userId, CancellationToken cancellationToken);
        Task<Guid> Save(string registrationNumber, string shortDescription);
    }
}

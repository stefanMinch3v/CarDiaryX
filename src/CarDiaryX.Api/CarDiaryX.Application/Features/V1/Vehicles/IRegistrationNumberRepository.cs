using CarDiaryX.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Vehicles
{
    public interface IRegistrationNumberRepository
    {
        Task AddToUser(Guid registrationNumberId);
        Task<RegistrationNumber> Get(string registrationNumber, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<RegistrationNumber>> GetByUser(CancellationToken cancellationToken);
        Task<Guid> Save(string registrationNumber, string shortDescription, string vehicleType);
        Task<bool> DoesBelongToUser(string registrationNumber, CancellationToken cancellationToken);
    }
}

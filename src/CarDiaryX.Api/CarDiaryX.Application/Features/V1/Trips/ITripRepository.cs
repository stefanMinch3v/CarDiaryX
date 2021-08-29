using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Application.Features.V1.Trips
{
    public interface ITripRepository
    {
        Task Add(
            string userId,
            string registrationNumber,
            DateTimeOffset departureDate,
            DateTimeOffset arrivalDate,
            AddressInputModel departureAddress,
            AddressInputModel arrivalAddress,
            int? distance = null,
            decimal? cost = null,
            string note = null);

        Task<IReadOnlyCollection<Trip>> GetAll(string userId, CancellationToken cancellationToken);

        Task<Trip> Get(int id, string userId, CancellationToken cancellationToken);

        Task Update(
            int id,
            string userId,
            string registrationNumber,
            DateTimeOffset departureDate,
            DateTimeOffset arrivalDate,
            AddressInputModel departureAddress,
            AddressInputModel arrivalAddress,
            int? distance = null,
            decimal? cost = null,
            string note = null);

        Task Delete(int id, string userId);
    }
}

using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.Vehicles;
using System;
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

        Task<PagingModel<Trip>> GetAll(
            string userId,
            CancellationToken cancellationToken,
            int page = ApplicationConstants.PAGE,
            int pageSize = ApplicationConstants.PAGE_SIZE);

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

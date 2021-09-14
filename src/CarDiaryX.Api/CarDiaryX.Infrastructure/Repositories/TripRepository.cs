using CarDiaryX.Application.Features.V1.Trips;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class TripRepository : ITripRepository
    {
        private readonly CarDiaryXDbContext dbContext;

        public TripRepository(CarDiaryXDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task Add(
            string userId,
            string registrationNumber,
            DateTimeOffset departureDate,
            DateTimeOffset arrivalDate,
            AddressInputModel departureAddress,
            AddressInputModel arrivalAddress,
            int? distance = null,
            decimal? cost = null,
            string note = null)
        {
            var trip = new Trip
            {
                DepartureAddress = departureAddress?.Name,
                DepartureAddressX = departureAddress?.X,
                DepartureAddressY = departureAddress?.Y,
                DepartureDate = departureDate,
                ArrivalAddress = arrivalAddress?.Name,
                ArrivalAddressX = arrivalAddress?.X,
                ArrivalAddressY = arrivalAddress?.Y,
                ArrivalDate = arrivalDate,
                Cost = cost,
                Distance = distance,
                Note = note,
                RegistrationNumber = registrationNumber,
                UserId = userId
            };

            this.dbContext.Trips.Add(trip);
            return this.dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id, string userId)
        {
            var trip = await this.dbContext.Trips
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip is null)
            {
                return;
            }

            this.dbContext.Trips.Remove(trip);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<(IReadOnlyCollection<Trip> Trips, int TotalCount)> GetAll(
            string userId,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 7)
        {
            var tripsQuery = this.dbContext.Trips
                .Where(t => t.UserId == userId);

            var totalCount = await tripsQuery
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var trips = await tripsQuery
                .AsNoTracking()
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync(cancellationToken);

            return (trips, totalCount);
        }

        public async Task Update(
            int id,
            string userId,
            string registrationNumber,
            DateTimeOffset departureDate,
            DateTimeOffset arrivalDate,
            AddressInputModel departureAddress,
            AddressInputModel arrivalAddress,
            int? distance = null,
            decimal? cost = null,
            string note = null)
        {
            var trip = await this.dbContext.Trips
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip is null)
            {
                return;
            }

            trip.RegistrationNumber = registrationNumber;
            trip.DepartureDate = departureDate;
            trip.ArrivalDate = arrivalDate;
            trip.Distance = distance;
            trip.Cost = cost;
            trip.Note = note;
            trip.DepartureAddress = departureAddress?.Name;
            trip.DepartureAddressX = departureAddress?.X;
            trip.DepartureAddressY = departureAddress?.Y;
            trip.ArrivalAddress = arrivalAddress?.Name;
            trip.ArrivalAddressX = arrivalAddress?.X;
            trip.ArrivalAddressY = arrivalAddress?.Y;

            this.dbContext.Update(trip);
            await this.dbContext.SaveChangesAsync();
        }
    }
}

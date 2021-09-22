using CarDiaryX.Application.Features.V1.Trips;
using CarDiaryX.Application.Features.V1.Trips.InputModels;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common;
using CarDiaryX.Infrastructure.Common.Persistence;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class TripRepository : ITripRepository
    {
        private const string TABLE_NAME = "[dbo].[Trips]";
        private readonly CarDiaryXDbContext dbContext;
        private readonly IDbConnectionFactory dbFactory;

        public TripRepository(CarDiaryXDbContext dbContext, IDbConnectionFactory dbFactory)
        {
            this.dbContext = dbContext;
            this.dbFactory = dbFactory;
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

        public async Task<PagingModel<Trip>> GetAll(
            string userId,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 7)
        {
            var sql = $@"
                SELECT * FROM {TABLE_NAME}
                WHERE UserId = @userId
                ORDER BY Id DESC
                OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY;

                SELECT COUNT(*) FROM {TABLE_NAME}
                WHERE UserId = @userId";

            var parameters = new
            {
                userId = userId,
                page = (page - 1) * pageSize,
                pageSize = pageSize
            };

            using (var conn = this.dbFactory.GetConnection)
            {
                using (var multi = await conn.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)))
                {
                    var trips = multi.Read<Trip>();
                    var totalCount = multi.ReadFirstOrDefault<int>();
                    return new PagingModel<Trip>(trips.ToArray(), totalCount);
                }
            }
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

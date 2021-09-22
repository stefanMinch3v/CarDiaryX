using CarDiaryX.Application.Features.V1.VehicleServices;
using CarDiaryX.Application.Features.V1.VehicleServices.InputModels;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.VehicleServices;
using CarDiaryX.Infrastructure.Common;
using CarDiaryX.Infrastructure.Common.Helpers;
using CarDiaryX.Infrastructure.Common.Persistence;
using CarDiaryX.Infrastructure.Identity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class VehicleServicesRepository : IVehicleServicesRepository
    {
        private const string VEHICLE_SERVICES_TABLE_NAME = "[dbo].[VehicleServices]";
        private const string REVIEWS_TABLE_NAME = "[dbo].[Reviews]";
        private const string USERS_TABLE_NAME = "[dbo].[AspNetUsers]";
        private readonly CarDiaryXDbContext dbContext;
        private readonly IDbConnectionFactory dbFactory;

        public VehicleServicesRepository(CarDiaryXDbContext dbContext, IDbConnectionFactory dbFactory)
        {
            this.dbContext = dbContext;
            this.dbFactory = dbFactory;
        }

        public async Task<PagingModel<VehicleService>> GetAllShallow(CancellationToken cancellationToken, string searchText = null, int page = 1, int pageSize = 7)
        {
            var searchCondition = !string.IsNullOrWhiteSpace(searchText)
                ? $@"
                    WHERE vs.IsApproved = 1 
                        AND vs.[Name] LIKE CONCAT('%', @searchText, '%') 
                        OR vs.[Address] LIKE CONCAT('%', @searchText, '%')"
                : $@"WHERE vs.IsApproved = 1";

            var sql = $@"
                SELECT 
                    vs.Id,
	                vs.[Name], 
	                vs.[Description], 
	                vs.[Address], 
                    r.Id,
                    r.Ratings,
                    r.Prices,
                    r.VehicleServiceId
                FROM {VEHICLE_SERVICES_TABLE_NAME} AS vs
                LEFT JOIN {REVIEWS_TABLE_NAME} AS r
                    ON vs.Id = r.VehicleServiceId
                {searchCondition}
                ORDER BY vs.Id DESC
                OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY;

                SELECT COUNT(*) FROM {VEHICLE_SERVICES_TABLE_NAME} AS vs {searchCondition};";

            var parameters = new
            {
                searchText = searchText,
                page = (page - 1) * pageSize,
                pageSize = pageSize
            };

            using (var conn = this.dbFactory.GetConnection)
            {
                var vehicleServicesDict = new Dictionary<int, VehicleService>();

                using (var multi = await conn.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)))
                {
                    var result = multi.Read<VehicleService, Review, VehicleService>((vehicleServiceDb, reviewDb) =>
                    {
                        if (!vehicleServicesDict.TryGetValue(vehicleServiceDb.Id, out VehicleService vehicleService))
                        {
                            vehicleService = vehicleServiceDb;
                            vehicleService.Reviews = new HashSet<Review>();
                            vehicleServicesDict.Add(vehicleService.Id, vehicleService);
                        }

                        if (reviewDb != null)
                        {
                            var reivew = new Review
                            {
                                Id = reviewDb.Id,
                                VehicleServiceId = reviewDb.VehicleServiceId,
                                Ratings = reviewDb.Ratings,
                                Prices = reviewDb.Prices
                            };

                            vehicleService.Reviews.Add(reivew);
                        }

                        return vehicleService;
                    })
                    .Distinct()
                    .ToArray();

                    var totalCount = multi.ReadFirstOrDefault<int>();

                    return new PagingModel<VehicleService>(result, totalCount);
                }
            }
        }

        public async Task<PagingModel<Review>> GetAllReviews(CancellationToken cancellationToken, int page = 1, int pageSize = 7)
        {
            var sql = $@"
                SELECT 
                    r.Id,
                    r.[Name],
                    r.Ratings,
                    r.Prices,
                    r.VehicleServiceId,
                    r.CreatedOn,
                    u.Id,
                    u.FirstName,
                    u.LastName
                FROM {REVIEWS_TABLE_NAME} AS r
                LEFT JOIN {USERS_TABLE_NAME} AS u
                    ON r.CreatedBy = u.Id
                ORDER BY r.Id DESC
                OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY;

                SELECT COUNT(*) FROM {REVIEWS_TABLE_NAME};";

            var parameters = new
            {
                page = (page - 1) * pageSize,
                pageSize = pageSize
            };

            using (var conn = this.dbFactory.GetConnection)
            {
                using (var multi = await conn.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)))
                {
                    var result = multi.Read<Review, User, Review>((reviewDb, userDb) =>
                    {
                        reviewDb.CreatedBy = UserHelper.PrettifyUserNames(userDb);
                        return reviewDb;
                    })
                    .Distinct()
                    .ToArray();

                    var totalCount = multi.ReadFirstOrDefault<int>();

                    return new PagingModel<Review>(result, totalCount);
                }
            }
        }

        public async Task<VehicleService> GetShallow(int id, CancellationToken cancellationToken)
        {
            var sql = $@"
                SELECT
                    vs.Id,
                    vs.[Name],
                    vs.[Description],
                    r.Id,                    
                    r.[Name],
                    r.Ratings,
                    r.Prices,
                    r.CreatedOn,
                    u.Id,
                    u.FirstName,
                    u.LastName
                FROM {VEHICLE_SERVICES_TABLE_NAME} AS vs
                LEFT JOIN {REVIEWS_TABLE_NAME} AS r
                    ON r.VehicleServiceId = vs.Id
                LEFT JOIN {USERS_TABLE_NAME} AS u
                    ON u.Id = r.CreatedBy
                WHERE vs.Id = @id
                    AND vs.IsApproved = 1";

            var parameters = new { id };

            using (var conn = this.dbFactory.GetConnection)
            {
                var vehicleServicesDict = new Dictionary<int, VehicleService>();

                return (await conn.QueryAsync<VehicleService, Review, User, VehicleService>(
                    new CommandDefinition(sql, parameters, cancellationToken: cancellationToken),
                    (vehicleServiceDb, reviewDb, userDb) =>
                    {
                        if (!vehicleServicesDict.TryGetValue(vehicleServiceDb.Id, out VehicleService vehicleService))
                        {
                            vehicleService = vehicleServiceDb;
                            vehicleService.Reviews = new HashSet<Review>();
                            vehicleServicesDict.Add(vehicleService.Id, vehicleService);
                        }

                        if (reviewDb != null)
                        {
                            var reivew = new Review
                            {
                                Id = reviewDb.Id,
                                Name = reviewDb.Name,
                                Ratings = reviewDb.Ratings,
                                Prices = reviewDb.Prices,
                                CreatedOn = reviewDb.CreatedOn,
                                CreatedBy = UserHelper.PrettifyUserNames(userDb)
                            };

                            vehicleService.Reviews.Add(reivew);
                        }

                        return vehicleService;
                    })
                )
                .FirstOrDefault();
            }
        }

        public async Task<bool> Add(string name, string description, AddressInputModel address)
        {
            // guard
            if (address is null)
            {
                address = new AddressInputModel
                {
                    Name = string.Empty
                };
            }

            var existing = await this.dbContext.VehicleServices
                .AsNoTracking()
                .FirstOrDefaultAsync(vs => vs.Name == name || vs.Address == address.Name);

            if (existing != null)
            {
                return false;
            }

            var vehicleService = new VehicleService
            {
                Address = address.Name,
                AddressX = address.X,
                AddressY = address.Y,
                Name = name,
                Description = description,
                IsApproved = false
            };

            this.dbContext.VehicleServices.Add(vehicleService);
            return await this.dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddReview(string name, int vehicleServiceId, Ratings ratings, Prices prices)
        {
            var existingVehicleService = await this.dbContext.VehicleServices
                .AsNoTracking()
                .FirstOrDefaultAsync(vs => vs.Id == vehicleServiceId);

            if (existingVehicleService is null)
            {
                return false;
            }

            var review = new Review
            {
                Name = name,
                VehicleServiceId = vehicleServiceId,
                Ratings = ratings,
                Prices = prices
            };

            this.dbContext.Reviews.Add(review);
            return await this.dbContext.SaveChangesAsync() > 0;
        }
    }
}

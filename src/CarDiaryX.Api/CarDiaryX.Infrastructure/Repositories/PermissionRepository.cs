using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class PermissionRepository : IPermissionRepository
    {
        private readonly CarDiaryXDbContext dbContext;

        public PermissionRepository(CarDiaryXDbContext dbContext) 
            => this.dbContext = dbContext;

        public async Task AddDefault(string userId)
        {
            var permission = new Permission { UserId = userId };
            this.dbContext.Permissions.Add(permission);
            await this.dbContext.SaveChangesAsync();
        }

        public Task<Permission> GetByUser(string userId)
            => this.dbContext.Permissions.FirstOrDefaultAsync(p => p.UserId == userId);
    }
}

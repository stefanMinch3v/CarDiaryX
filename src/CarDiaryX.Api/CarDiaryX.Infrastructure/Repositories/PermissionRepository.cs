using CarDiaryX.Application.Contracts;
using CarDiaryX.Application.Features.V1.Vehicles;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Repositories
{
    internal class PermissionRepository : IPermissionRepository
    {
        private readonly CarDiaryXDbContext dbContext;
        private readonly ICurrentUser currentUser;

        public PermissionRepository(CarDiaryXDbContext dbContext, ICurrentUser currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        public Task AddDefault(string userId)
        {
            var permission = new Permission { UserId = userId };
            this.dbContext.Permissions.Add(permission);
            return this.dbContext.SaveChangesAsync();
        }

        public Task<Permission> GetByUser(CancellationToken cancellationToken)
            => this.dbContext.Permissions
                .AsNoTracking()    
                .FirstOrDefaultAsync(p => p.UserId == this.currentUser.UserId, cancellationToken);
    }
}

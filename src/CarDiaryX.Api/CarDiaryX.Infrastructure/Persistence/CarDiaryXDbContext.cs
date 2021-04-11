using CarDiaryX.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CarDiaryX.Infrastructure.Common.Persistence
{
    internal class CarDiaryXDbContext : IdentityDbContext<User>
    {
        public CarDiaryXDbContext(DbContextOptions<CarDiaryXDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}

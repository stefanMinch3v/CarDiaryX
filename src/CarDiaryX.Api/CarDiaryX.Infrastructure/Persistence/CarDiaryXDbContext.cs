using CarDiaryX.Application.Contracts;
using CarDiaryX.Domain.Common;
using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Infrastructure.Common.Persistence
{
    internal class CarDiaryXDbContext : IdentityDbContext<User>
    {
        private const string DEFAULT_SYSTEM = "CarDiaryX_System";
        private readonly ICurrentUser currentUser;

        public CarDiaryXDbContext(DbContextOptions<CarDiaryXDbContext> options, ICurrentUser currentUser)
            : base(options)
        {
            this.currentUser = currentUser;
        }

        public DbSet<VehicleInformation> VehicleInformations { get; set; }
        public DbSet<VehicleDMR> VehicleDMRs { get; set; } // need date time of last payed tax from history in order to calc and send notifications
        public DbSet<VehicleInspection> VehicleInspections { get; set; } // need to research when and how to go on car inspection in order to send notifications
        public DbSet<UserRegistrationNumbers> UserRegistrationNumbers { get; set; }
        public DbSet<RegistrationNumber> RegistrationNumbers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
            => this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e => (e.Entity is ICreatedEntity || e.Entity is IModifiedEntity)
                    && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                if (entry.State == EntityState.Added && entry.Entity is not IUser)
                {
                    var entity = (ICreatedEntity)entry.Entity;
                    entity.CreatedOn = DateTimeOffset.UtcNow;
                    entity.CreatedBy ??= this.currentUser?.UserId ?? DEFAULT_SYSTEM;
                }
                else if (entry.State == EntityState.Modified)
                {
                    var entity = (IModifiedEntity)entry.Entity;
                    entity.ModifiedOn = DateTimeOffset.UtcNow;
                    entity.ModifiedBy ??= this.currentUser?.UserId ?? DEFAULT_SYSTEM;
                }
            }
        }
    }
}

using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using CarDiaryX.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Common.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.RegistrationNumbers)
                .WithOne(rn => (User)rn.User)
                .HasForeignKey(rn => rn.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(u => u.ModifiedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_MODIFIED_BY_MAX_LENGTH);

            builder
                .HasOne(u => u.Permission)
                .WithOne(p => (User)p.User)
                .HasForeignKey<Permission>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

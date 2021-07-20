using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class UserRegistrationNumbersConfiguration : IEntityTypeConfiguration<UserRegistrationNumbers>
    {
        public void Configure(EntityTypeBuilder<UserRegistrationNumbers> builder)
        {
            builder
                .HasKey(r => new { r.UserId, r.RegistrationNumberId });

            builder
                .Property(r => r.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_BY_MAX_LENGTH);

            //builder
            //    .HasOne(ur => ur.RegistrationNumber)
            //    .WithMany(rn => rn.Users)
            //    .HasForeignKey(ur => ur.RegistrationNumberId);

            //builder
            //    .HasOne(ur => ur.User)
            //    .WithMany(u => u.RegistrationNumbers)
            //    .HasForeignKey(ur => ur.UserId);
        }
    }
}

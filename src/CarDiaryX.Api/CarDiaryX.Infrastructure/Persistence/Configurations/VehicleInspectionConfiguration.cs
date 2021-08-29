using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class VehicleInspectionConfiguration : IEntityTypeConfiguration<VehicleInspection>
    {
        public void Configure(EntityTypeBuilder<VehicleInspection> builder)
        {
            builder
                .HasKey(v => v.Id);

            builder
                .Property(v => v.RegistrationNumber)
                .IsRequired();

            builder
                .Property(v => v.RegistrationNumber)
                .HasMaxLength(InfrastructureConstants.REGISTRATION_NUMBER_MAX_LENGTH);

            builder
                .Property(v => v.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_MODIFIED_BY_MAX_LENGTH);

            builder
                .Property(v => v.ModifiedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_MODIFIED_BY_MAX_LENGTH);

            builder
                .HasIndex(v => v.RegistrationNumber)
                .IsUnique();
        }
    }
}

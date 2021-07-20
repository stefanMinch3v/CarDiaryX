using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class VehicleInformationConfiguration : IEntityTypeConfiguration<VehicleInformation>
    {
        public void Configure(EntityTypeBuilder<VehicleInformation> builder)
        {
            builder
                .HasKey(v => v.Id);

            builder
                .Property(v => v.RegistrationNumber)
                .IsRequired();

            builder
                .Property(v => v.DataTsId)
                .IsRequired();

            builder
                .Property(v => v.DataId)
                .IsRequired();

            builder
                .Property(v => v.RegistrationNumber)
                .HasMaxLength(InfrastructureConstants.REGISTRATION_NUMBER_MAX_LENGTH);

            builder
                .Property(v => v.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_BY_MAX_LENGTH);

            builder
                .Property(v => v.ModifiedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_BY_MAX_LENGTH);

            builder
                .HasIndex(v => v.RegistrationNumber)
                .IsUnique();
        }
    }
}

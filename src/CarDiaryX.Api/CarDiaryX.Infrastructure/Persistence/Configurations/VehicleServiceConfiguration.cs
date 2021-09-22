using CarDiaryX.Domain.VehicleServices;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class VehicleServiceConfiguration : IEntityTypeConfiguration<VehicleService>
    {
        public void Configure(EntityTypeBuilder<VehicleService> builder)
        {
            builder
                .HasKey(vs => vs.Id);

            builder
                .Property(vs => vs.Name)
                .HasMaxLength(InfrastructureConstants.VehicleServices.NAME_MAX_LENGTH)
                .IsRequired();

            builder
                .HasIndex(vs => vs.Name)
                .IsUnique();

            builder
                .Property(vs => vs.Address)
                .HasMaxLength(InfrastructureConstants.ADDRESS_MAX_LENGTH)
                .IsRequired();

            builder
                .HasIndex(vs => vs.Address)
                .IsUnique();

            builder
                .Property(vs => vs.AddressX)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH)
                .IsRequired();

            builder
                .Property(vs => vs.AddressY)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH)
                .IsRequired();

            builder
                .Property(vs => vs.Description)
                .HasMaxLength(InfrastructureConstants.VehicleServices.DESCRIPTION_MAX_LENGTH)
                .IsRequired();

            builder
                .Property(vs => vs.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_MODIFIED_BY_MAX_LENGTH);

            builder
                .HasMany(vs => vs.Reviews)
                .WithOne(r => r.VehicleService)
                .HasForeignKey(r => r.VehicleServiceId);

            builder
                .Property(vs => vs.IsApproved)
                .HasDefaultValue(false);
        }
    }
}

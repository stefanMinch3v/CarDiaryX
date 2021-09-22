using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using CarDiaryX.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => (User)t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(t => t.Cost)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            builder
                .Property(t => t.Distance)
                .IsRequired(false);

            builder
                .Property(t => t.Note)
                .IsRequired(false)
                .HasMaxLength(InfrastructureConstants.Trips.NOTE_MAX_LENGTH);

            builder
                .Property(t => t.ArrivalAddress)
                .IsRequired(true)
                .HasMaxLength(InfrastructureConstants.ADDRESS_MAX_LENGTH);

            builder
                .Property(t => t.DepartureAddress)
                .IsRequired(true)
                .HasMaxLength(InfrastructureConstants.ADDRESS_MAX_LENGTH);

            builder
                .Property(t => t.DepartureAddressX)
                .IsRequired(false)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH);

            builder
                .Property(t => t.DepartureAddressY)
                .IsRequired(false)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH);

            builder
                .Property(t => t.ArrivalAddressX)
                .IsRequired(false)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH);

            builder
                .Property(t => t.ArrivalAddressY)
                .IsRequired(false)
                .HasMaxLength(InfrastructureConstants.COORDINATES_MAX_LENGTH);

            builder
                .Property(t => t.ArrivalDate)
                .IsRequired(true);

            builder
                .Property(t => t.DepartureDate)
                .IsRequired(true);

            builder
                .Property(t => t.RegistrationNumber)
                .IsRequired(true)
                .HasMaxLength(InfrastructureConstants.REGISTRATION_NUMBER_MAX_LENGTH);
        }
    }
}

using CarDiaryX.Domain.Vehicles;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class RegistrationNumberConfiguration : IEntityTypeConfiguration<RegistrationNumber>
    {
        public void Configure(EntityTypeBuilder<RegistrationNumber> builder)
        {
            //builder
            //    .Property(r => r.Id)
            //    .HasMaxLength(450)
            //    .HasDefaultValueSql("LOWER(NEWID())");

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(r => r.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_BY_MAX_LENGTH);

            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Number)
                .IsRequired()
                .HasMaxLength(InfrastructureConstants.REGISTRATION_NUMBER_MAX_LENGTH);

            builder
                .Property(r => r.ShortDescription)
                .HasMaxLength(InfrastructureConstants.SHORT_DESCRIPTION_MAX_LENGTH);

            builder
                .HasIndex(r => r.Number)
                .IsUnique();

            builder
                .HasMany(rn => rn.Users)
                .WithOne(urn => urn.RegistrationNumber)
                .HasForeignKey(urn => urn.RegistrationNumberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

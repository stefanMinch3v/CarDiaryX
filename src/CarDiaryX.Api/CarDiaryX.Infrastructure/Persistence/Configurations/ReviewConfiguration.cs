using CarDiaryX.Domain.VehicleServices;
using CarDiaryX.Infrastructure.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDiaryX.Infrastructure.Persistence.Configurations
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(InfrastructureConstants.VehicleServices.NAME_REVIEW_MAX_LENGTH);

            builder
                .Property(r => r.CreatedBy)
                .HasMaxLength(InfrastructureConstants.CREATED_MODIFIED_BY_MAX_LENGTH);

            builder
                .Property(r => r.Prices)
                .IsRequired();

            builder
                .Property(r => r.Ratings)
                .IsRequired();
        }
    }
}

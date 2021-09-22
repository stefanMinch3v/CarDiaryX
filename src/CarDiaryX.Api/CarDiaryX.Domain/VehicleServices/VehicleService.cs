using CarDiaryX.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDiaryX.Domain.VehicleServices
{
    public class VehicleService : Entity<int>, ICreatedEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string AddressX { get; set; }

        public string AddressY { get; set; }

        public bool IsApproved { get; set; }

        public HashSet<Review> Reviews { get; set; } = new HashSet<Review>();

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int GetAverageReviewRatings()
        {
            if (this.GetNumOfReviews() == 0)
            {
                return 0;
            }

            var ratingsSum = (double)this.Reviews.Sum(r => (int)r.Ratings);

            return (int)Math.Round(ratingsSum / this.Reviews.Count, MidpointRounding.AwayFromZero);
        }

        public int GetAverageReviewPrices()
        {
            if (this.GetNumOfReviews() == 0)
            {
                return 0;
            }

            var pricesSum = (double)this.Reviews.Sum(r => (int)r.Prices);

            return (int)Math.Round(pricesSum / this.Reviews.Count, MidpointRounding.AwayFromZero);
        }

        public int GetNumOfReviews()
            => this.Reviews.Count;
    }
}

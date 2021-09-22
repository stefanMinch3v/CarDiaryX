using CarDiaryX.Domain.Common;
using System;

namespace CarDiaryX.Domain.VehicleServices
{
    public class Review : Entity<int>, ICreatedEntity
    {
        public string Name { get; set; }

        public Ratings Ratings { get; set; }

        public Prices Prices { get; set; }

        public int VehicleServiceId { get; set; }

        public VehicleService VehicleService { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}

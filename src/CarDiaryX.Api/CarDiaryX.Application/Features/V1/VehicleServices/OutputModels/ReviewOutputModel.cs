using System;

namespace CarDiaryX.Application.Features.V1.VehicleServices.OutputModels
{
    public class ReviewOutputModel
    {
        public string Name { get; set; }

        public int Ratings { get; set; }

        public int Prices { get; set; }

        public string CreatedByUser { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}

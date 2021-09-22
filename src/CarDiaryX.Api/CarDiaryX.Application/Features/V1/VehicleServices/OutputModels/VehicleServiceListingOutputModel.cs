namespace CarDiaryX.Application.Features.V1.VehicleServices.OutputModels
{
    public class VehicleServiceListingOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int AverageReviewRatings { get; set; }
        public int AverageReviewPrices { get; set; }
        public int NumberOfReviews { get; set; }
    }
}

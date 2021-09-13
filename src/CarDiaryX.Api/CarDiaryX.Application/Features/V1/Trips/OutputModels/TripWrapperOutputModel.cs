using System.Collections.Generic;

namespace CarDiaryX.Application.Features.V1.Trips.OutputModels
{
    public class TripWrapperOutputModel
    {
        public IReadOnlyCollection<TripOutputModel> Trips { get; set; }
        public int TotalCount { get; set; }
    }
}

using System.Collections.Generic;

namespace CarDiaryX.Application.Features.V1.VehicleServices.OutputModels
{
    public class VehicleServiceOutputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<ReviewOutputModel> Reviews { get; set; }
    }
}

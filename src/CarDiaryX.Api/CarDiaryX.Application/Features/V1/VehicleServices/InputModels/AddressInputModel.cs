using CarDiaryX.Application.Common.Models;

namespace CarDiaryX.Application.Features.V1.VehicleServices.InputModels
{
    public class AddressInputModel : IAddress
    {
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}

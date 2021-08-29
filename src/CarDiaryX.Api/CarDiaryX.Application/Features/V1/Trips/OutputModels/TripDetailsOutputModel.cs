using CarDiaryX.Application.Features.V1.Trips.InputModels;
using System;

namespace CarDiaryX.Application.Features.V1.Trips.OutputModels
{
    public class TripDetailsOutputModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }
        public int? Distance { get; set; }
        public decimal? Cost { get; set; }
        public string Note { get; set; }
        public AddressInputModel DepartureAddress { get; set; }
        public AddressInputModel ArrivalAddress { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

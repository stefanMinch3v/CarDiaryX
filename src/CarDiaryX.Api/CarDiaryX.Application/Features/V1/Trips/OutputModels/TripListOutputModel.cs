using System;

namespace CarDiaryX.Application.Features.V1.Trips.OutputModels
{
    public class TripListOutputModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }
        public int? Distance { get; set; }
        public decimal? Cost { get; set; }
        public string DepartureAddress { get; set; }
        public string ArrivalAddress { get; set; }
    }
}

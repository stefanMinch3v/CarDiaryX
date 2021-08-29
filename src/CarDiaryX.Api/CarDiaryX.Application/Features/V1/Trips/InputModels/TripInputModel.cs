using System;

namespace CarDiaryX.Application.Features.V1.Trips.InputModels
{
    public class TripInputModel
    {
        public DateTimeOffset DepartureDate { get; set; }

        public DateTimeOffset ArrivalDate { get; set; }

        public AddressInputModel DepartureAddress { get; set; }

        public AddressInputModel ArrivalAddress { get; set; }

        public int? Distance { get; set; }

        public decimal? Cost { get; set; }

        public string Note { get; set; }

        public string RegistrationNumber { get; set; }
    }
}

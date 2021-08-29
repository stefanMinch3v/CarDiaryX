using CarDiaryX.Domain.Common;
using System;

namespace CarDiaryX.Domain.Vehicles
{
    public class Trip : Entity<int>, IAuditableEntity
    {
        public string RegistrationNumber { get; set; }
        public DateTimeOffset DepartureDate { get; set; }
        public string DepartureAddress { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }
        public string ArrivalAddress { get; set; }
        public int? Distance { get; set; }
        public decimal? Cost { get; set; }
        public string Note { get; set; }
        public string DepartureAddressX { get; set; }
        public string DepartureAddressY { get; set; }
        public string ArrivalAddressX { get; set; }
        public string ArrivalAddressY { get; set; }
        public string UserId { get; set; }
        public IUser User { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

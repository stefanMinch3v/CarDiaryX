using CarDiaryX.Domain.Common;
using System;

namespace CarDiaryX.Domain.Vehicles
{
    public class VehicleInspection : Entity<long>, IAuditableEntity
    {
        public string RegistrationNumber { get; set; } // unique index
        public string JsonData { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

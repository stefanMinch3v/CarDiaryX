using CarDiaryX.Domain.Common;
using System;

namespace CarDiaryX.Domain.Vehicles
{
    public class UserRegistrationNumbers : ICreatedEntity
    {
        public string UserId { get; set; }
        public IUser User { get; set; }
        public Guid RegistrationNumberId { get; set; }
        public RegistrationNumber RegistrationNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

using CarDiaryX.Domain.Common;
using System;
using System.Collections.Generic;

namespace CarDiaryX.Domain.Vehicles
{
    public class RegistrationNumber : Entity<Guid>, IAuditableEntity
    {
        public RegistrationNumber()
        {
            this.Id = Guid.NewGuid();
        }

        public string Number { get; set; }

        public string ShortDescription { get; set; }

        public HashSet<UserRegistrationNumbers> Users { get; set; } = new HashSet<UserRegistrationNumbers>();

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

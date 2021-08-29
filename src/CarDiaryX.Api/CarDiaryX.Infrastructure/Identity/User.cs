using CarDiaryX.Domain.Vehicles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CarDiaryX.Infrastructure.Identity
{
    public class User : IdentityUser, IUser
    {
        internal User(string email, string firstName, string lastName)
            : base(email)
        {
            base.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public HashSet<UserRegistrationNumbers> RegistrationNumbers { get; set; } = new HashSet<UserRegistrationNumbers>();
        public HashSet<Trip> Trips { get; set; } = new HashSet<Trip>();
        public Permission Permission { get; set; }

        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

using CarDiaryX.Domain.Vehicles.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CarDiaryX.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        internal User(string email, string phoneNumber = null)
            : base(email)
        {
            base.Email = email;
            base.PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public HashSet<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}

using Microsoft.AspNetCore.Identity;

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

        // public HashSet<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}

using Microsoft.AspNetCore.Identity;

namespace CarDiaryX.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        internal User(string email, string firstName, string lastName, int age)
            : base(email)
        {
            base.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        // public HashSet<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}

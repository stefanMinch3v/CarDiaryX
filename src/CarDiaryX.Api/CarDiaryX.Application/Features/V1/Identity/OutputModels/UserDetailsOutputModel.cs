namespace CarDiaryX.Application.Features.V1.Identity.OutputModels
{
    public class UserDetailsOutputModel
    {
        public UserDetailsOutputModel(string email, string firstName, string lastName, int age)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }

        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
    }
}

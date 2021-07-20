namespace CarDiaryX.Application.Features.V1.Identity.OutputModels
{
    public class UserDetailsOutputModel
    {
        public UserDetailsOutputModel(string email, string firstName, string lastName)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}

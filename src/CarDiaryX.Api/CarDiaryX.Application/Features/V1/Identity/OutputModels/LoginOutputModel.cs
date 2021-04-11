namespace CarDiaryX.Application.Features.V1.Identity.OutputModels
{
    public class LoginOutputModel
    {
        public LoginOutputModel(string token)
            => this.Token = token;

        public string Token { get; }
    }
}

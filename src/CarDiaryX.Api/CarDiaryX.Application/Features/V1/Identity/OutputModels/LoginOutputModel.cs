using System;

namespace CarDiaryX.Application.Features.V1.Identity.OutputModels
{
    public class LoginOutputModel
    {
        public LoginOutputModel(string token, DateTime expiration)
        {
            this.Token = token;
            this.Expiration = expiration;
        }

        public string Token { get; }

        public DateTime Expiration { get; }
    }
}

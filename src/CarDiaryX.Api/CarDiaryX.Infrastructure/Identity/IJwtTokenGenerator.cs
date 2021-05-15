using System;

namespace CarDiaryX.Infrastructure.Identity
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime Expiration) GenerateToken(User user);
    }
}

using Microsoft.Extensions.Configuration;

namespace CarDiaryX.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");
    }
}

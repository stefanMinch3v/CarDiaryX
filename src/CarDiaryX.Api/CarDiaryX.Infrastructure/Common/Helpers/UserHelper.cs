using CarDiaryX.Infrastructure.Identity;

namespace CarDiaryX.Infrastructure.Common.Helpers
{
    internal static class UserHelper
    {
        public static string PrettifyUserNames(User user)
        {
            if (user is null)
            {
                return "Unknown";
            }

            return $"{user.FirstName} {user.LastName}";
        }
    }
}

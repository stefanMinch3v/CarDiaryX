namespace CarDiaryX.Application.Common.Constants
{
    public static class ApplicationConstants
    {
        public static class Users
        {
            public const int EMAIL_MIN_LENGTH = 6;
            public const int EMAIL_MAX_LENGTH = 60;
            public const int NAME_MIN_LENGTH = 2;
            public const int NAME_MAX_LENGTH = 100;
            public const int PASSWORD_MIN_LENGTH = 6;
            public const int PASSWORD_MAX_LENGTH = 100;
        }

        public static class Vehicles
        {
            public const int PLATES_MAX_SIZE = 7;
        }

        public static class External
        {
            public const string SERVER_IS_NOT_RESPONDING = "Could not connect to the external server. Please try again later.";
            public const string NO_RESULTS_FOUND_ON_THE_SERVER = "No results found on the external server.";
        }
    }
}

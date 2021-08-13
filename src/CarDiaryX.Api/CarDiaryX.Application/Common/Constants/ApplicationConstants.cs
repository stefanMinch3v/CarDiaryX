namespace CarDiaryX.Application.Common.Constants
{
    public static class ApplicationConstants
    {
        public const string HALTED_FEATURES = "Excluded from the current version. Should be added when we get enough customers to pay for this feautre.";

        internal static class Users
        {
            public const int EMAIL_MIN_LENGTH = 6;
            public const int EMAIL_MAX_LENGTH = 60;
            public const int NAME_MIN_LENGTH = 2;
            public const int NAME_MAX_LENGTH = 100;
            public const int PASSWORD_MIN_LENGTH = 6;
            public const int PASSWORD_MAX_LENGTH = 100;
        }

        internal static class Vehicles
        {
            public const int PLATES_MAX_SIZE = 7;
            public const string DELETED_VEHICLE_FROM_GARAGE = "The vehicle has been deleted from your garage.";
        }

        internal static class Permissions
        {
            public const string ACCOUNT_HAS_NO_PERMISSIONS = "This account has no permissions to see the data.";
        }

        internal static class External
        {
            public const string SERVER_IS_NOT_RESPONDING = "Could not connect to the external server. Please try again later.";
            public const string NO_RESULTS_FOUND_ON_THE_SERVER = "No results found on the external server.";
            public const string INVALID_TSID_DATAID_PROPERTIES = "TsId/DataId are not found in the database or not saved correctly for registration number: {0}.";
        }
    }
}

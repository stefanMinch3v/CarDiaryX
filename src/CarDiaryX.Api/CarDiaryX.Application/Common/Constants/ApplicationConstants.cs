namespace CarDiaryX.Application.Common.Constants
{
    internal static class ApplicationConstants
    {
        public const string HALTED_FEATURES = "Excluded from the current version. Should be added when we get enough customers to pay for this feautre.";
        public const int PAGE = 1;
        public const int PAGE_SIZE = 7;
        public const int ADDRESS_MIN_LENGTH = 2;
        public const int ADDRESS_MAX_LENGTH = 150;
        public const int COORDINATES_MAX_LENGTH = 30;

        public static class Users
        {
            public const int EMAIL_MIN_LENGTH = 6;
            public const int EMAIL_MAX_LENGTH = 60;
            public const int NAME_MIN_LENGTH = 2;
            public const int NAME_MAX_LENGTH = 100;
            public const int PASSWORD_MIN_LENGTH = 6;
            public const int PASSWORD_MAX_LENGTH = 100;
            public const string INVALID_PASSWORD_EMPTY = "Password cannot be empty.";
            public const string INVALID_PASSWORD_LENGTH = "Password must be between 6 and 100 symbols.";
            public const string INVALID_FIRST_NAME_EMPTY = "First name cannot be empty.";
            public const string INVALID_FIRST_NAME_LENGTH = "First name must be between 2 and 100 symbols.";
            public const string INVALID_LAST_NAME_EMPTY = "Last name cannot be empty.";
            public const string INVALID_LAST_NAME_LENGTH = "Last name must be between 2 and 100 symbols.";
            public const string INVALID_EMAIL_EMPTY = "Email cannot be empty.";
            public const string INVALID_EMAIL_FORMAT = "Email format is invalid.";
            public const string INVALID_EMAIL_LENGTH = "Email must be between 6 and 60 symbols.";
        }

        public static class Vehicles
        {
            public const string INVALID_VEHICLE_NOT_BELONGING_TO_USER_GARAGE = "Vehicle with registration number: '{0}' does not exist in your garage.";
            public const int PLATES_MAX_SIZE = 7;
            public const string DELETED_VEHICLE_FROM_GARAGE = "The vehicle has been deleted from your garage.";
            public const string INVALID_PLATES_EMPTY = "Registration number cannot be empty.";
            public const string INVALID_PLATES_MAX_SIZE = "Registration number maximum length is 7.";
        }

        public static class Permissions
        {
            public const string ACCOUNT_HAS_NO_PERMISSIONS = "This account has no permissions to see the data.";
        }

        public static class Trips
        {
            public const string INVALID_TRIP_NULL = "Empty trip data is not allowed.";
            public const int DISTANCE_MAX_LENGTH = 10_000_000;
            public const int COST_MAX_LENGTH = 10_000_000;
            public const int NOTE_MAX_LENGTH = 250;
            public const string INVALID_COST_LENGTH = "Cost cannot be more than 10 million.";
            public const string INVALID_NOTE_LENGTH = "Note cannot be more than 250 symbols.";
            public const string INVALID_DISTANCE_LENGTH = "Distance cannot be more than 10 million.";
            public const string INVALID_DEPARTURE_ARRIVAL_DATES = "Departure must end before arrival date.";
            public const string INVALID_DEPARTURE_ADDRESS_EMPTY = "Departure address cannot be empty.";
            public const string INVALID_DEPARTURE_ADDRESS_NAME = "Departure address' name must be between 2 and 150 symbols.";
            public const string INVALID_DEPARTURE_ADDRESS_COORDINATES = "Departure address' coordinates (X,Y) have to be either both provided or none and can be maximum 30 symbols long.";
            public const string INVALID_ARRIVAL_ADDRESS_EMPTY = "Arrival address cannot be empty.";
            public const string INVALID_ARRIVAL_ADDRESS_NAME = "Arrival address' name must be between 2 and 150 symbols.";
            public const string INVALID_ARRIVAL_ADDRESS_COORDINATES = "Arrival address' coordinates (X,Y) have to be either both provided or none and can be maximum 30 symbols long.";
        }

        public static class External
        {
            public const string SERVER_IS_NOT_RESPONDING = "Could not connect to the external server. Please try again later.";
            public const string NO_RESULTS_FOUND_ON_THE_SERVER = "No results found on the external server.";
            public const string INVALID_TSID_DATAID_PROPERTIES = "TsId/DataId are not found in the database or not saved correctly for registration number: {0}.";
        }

        public static class VehicleServices
        {
            public const string INVALID_ADDRESS_EMPTY = "Address cannot be empty.";
            public const string INVALID_NAME_REVIEW = "Review must be between 2 and 100 symbols.";
            public const string INVALID_NAME_REVIEW_EMPTY = "Review cannot be empty.";
            public const string INVALID_DESCRIPTION_EMPTY = "Description cannot be empty.";
            public const string INVALID_DESCRIPTION = "Description must be between 5 and 150 symbols.";
            public const int NAME_REVIEW_MAX_LENGTH = 100;
            public const int NAME_MAX_LENGTH = 30;
            public const int NAME_MIN_LENGTH = 2;
            public const int DESCRIPTION_MAX_LENGTH = 150;
            public const int DESCRIPTION_MIN_LENGTH = 5;
            public const string INVALID_ADDRESS = "Address must be between 2 and 150 symbols.";
            public const string INVALID_ADDRESS_COORDINATES = "Address' coordinates (X,Y) have to be either both provided or none and can be maximum 30 symbols long.";
            public const string INVALID_NAME_EMPTY = "Name cannot be empty.";
            public const string INVALID_NAME = "Name must be between 2 and 30 symbols.";
            public const string INVALID_REVIEW_PRICES = "Price must be one of the following: '{0}'.";
            public const string INVALID_REVIEW_RATINGS = "Rating must be one of the following: '{0}'.";
            public const string INVALID_REVIEW_VEHICLE_SERVICE_TO_ATTACH = "Trying to attach review to an unexisting vehicle service.";
            public const string INVALID_ALREADY_EXISTING_SERVICE = "Vehicle service with the same 'Name' or 'Address' already exists.";
        }
    }
}

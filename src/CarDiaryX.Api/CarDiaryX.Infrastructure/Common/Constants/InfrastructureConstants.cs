namespace CarDiaryX.Infrastructure.Common.Constants
{
    public class InfrastructureConstants
    {
        public const string UNEXISTING_USER = "User does not exist in the system.";
        public const string INVALID_LOGIN_ERROR_MESSAGE = "Invalid credentials.";
        public const string INVALID_CONFIRM_PASSWORD_ERROR_MESSAGE = "Invalid password.";
        public const int REGISTRATION_NUMBER_MAX_LENGTH = 20;
        public const int CREATED_BY_MAX_LENGTH = 450;
        public const int SHORT_DESCRIPTION_MAX_LENGTH = 150;
        public const string DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_VI = "Cannot insert duplicate key row in object 'dbo.VehicleInformations' with unique index";
        public const string DUPLICATE_KEY_FOR_REGISTRATION_NUMBER_EXCEPTION_MESSAGE_IN_RN = "Cannot insert duplicate key row in object 'dbo.RegistrationNumbers' with unique index";
        public const string SAVED_REGISTRATION_NUMBER_EXCEPTION_IN_DUPLICATE_AND_UNEXISTING_STATE = "Trying to fetch existing 'RegistrationNumber' failed, the entry has been deleted right before fetching in 'catch' state of 'try catch' block.";
    }
}

using CarDiaryX.Application.Common.Exceptions;

namespace CarDiaryX.Application.Common
{
    public static class Guard
    {
        public static void AgainstEmptyString<TException>(string value, string name = "Value")
            where TException : BaseDomainException, new()
        {
            if (!string.IsNullOrEmpty(value))
            {
                return;
            }

            ThrowException<TException>($"{name} cannot be null ot empty.");
        }

        private static void ThrowException<TException>(string message)
           where TException : BaseDomainException, new()
        {
            var exception = new TException
            {
                Message = message
            };

            throw exception;
        }
    }
}

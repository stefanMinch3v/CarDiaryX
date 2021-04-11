using System;

namespace CarDiaryX.Application.Common.Exceptions
{
    public abstract class BaseDomainException : Exception
    {
        private string message;

        public new string Message
        {
            get => message ?? base.Message;
            set => message = value;
        }
    }
}

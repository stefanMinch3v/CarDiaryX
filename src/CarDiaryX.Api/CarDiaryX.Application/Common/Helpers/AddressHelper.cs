using CarDiaryX.Application.Common.Constants;
using CarDiaryX.Application.Common.Models;

namespace CarDiaryX.Application.Common.Helpers
{
    public static class AddressHelper
    {
        public static bool HasValidCoordinates(IAddress address)
        {
            if (address is null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(address.X) && string.IsNullOrEmpty(address.Y))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(address.X) && !string.IsNullOrEmpty(address.Y))
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(address.X) && !string.IsNullOrEmpty(address.Y))
            {
                if (address.X.Length > ApplicationConstants.COORDINATES_MAX_LENGTH
                    || address.Y.Length > ApplicationConstants.COORDINATES_MAX_LENGTH)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasValidName(IAddress address)
        {
            if (address is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(address.Name)
                || address.Name.Length > ApplicationConstants.ADDRESS_MAX_LENGTH
                || address.Name.Length < ApplicationConstants.ADDRESS_MIN_LENGTH)
            {
                return false;
            }

            return true;
        }
    }
}

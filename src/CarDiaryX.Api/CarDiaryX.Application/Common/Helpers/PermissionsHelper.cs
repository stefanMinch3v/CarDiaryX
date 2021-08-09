using CarDiaryX.Domain.Vehicles;

namespace CarDiaryX.Application.Common.Helpers
{
    public static class PermissionsHelper
    {
        public static bool IsPaidUser(Permission userPermission)
            => userPermission?.PermissionType == PermissionType.Premium 
                || userPermission?.PermissionType == PermissionType.Professional;
    }
}

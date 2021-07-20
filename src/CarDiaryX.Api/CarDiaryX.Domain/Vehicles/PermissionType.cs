namespace CarDiaryX.Domain.Vehicles
{
    public enum PermissionType
    {
        Free = 1, // 1 vehicle only
        Premium = 2, // up to 4
        Professional = 3 // unlimited vehicles but limit the mobile devices somehow (register device id on login)?
    }
}

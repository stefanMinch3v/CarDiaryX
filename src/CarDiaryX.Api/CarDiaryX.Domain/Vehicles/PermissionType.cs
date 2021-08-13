namespace CarDiaryX.Domain.Vehicles
{
    public enum PermissionType
    {
        Free = 1, // 1 vehicle only, limited inspections and dmr
        Premium = 2, // up to 4
        Professional = 3 // figure out what this should be cuz unlimited vehicles is not an option considered the fact the we pay for api call
    }
}

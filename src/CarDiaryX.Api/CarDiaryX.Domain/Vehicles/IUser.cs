using CarDiaryX.Domain.Common;
using System.Collections.Generic;

namespace CarDiaryX.Domain.Vehicles
{
    public interface IUser : IModifiedEntity
    {
        string Id { get; set; }
        HashSet<UserRegistrationNumbers> RegistrationNumbers { get; set; }
        HashSet<Trip> Trips { get; set; }
        Permission Permission { get; set; }
    }
}

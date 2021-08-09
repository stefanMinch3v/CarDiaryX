using CarDiaryX.Domain.Common;
using System;

namespace CarDiaryX.Domain.Vehicles
{
    public class Permission : Entity<int>, IAuditableEntity
    {
        public string UserId { get; set; }
        public IUser User { get; set; }
        public PermissionType PermissionType { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}

using System;

namespace CarDiaryX.Domain.Common
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }

        DateTimeOffset? CreatedOn { get; set; }

        string ModifiedBy { get; set; }

        DateTimeOffset? ModifiedOn { get; set; }
    }
}

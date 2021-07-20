using System;

namespace CarDiaryX.Domain.Common
{
    public interface IModifiedEntity
    {
        string ModifiedBy { get; set; }

        DateTimeOffset? ModifiedOn { get; set; }
    }
}

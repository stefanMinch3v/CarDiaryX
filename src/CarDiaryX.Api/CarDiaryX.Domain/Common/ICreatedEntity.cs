using System;

namespace CarDiaryX.Domain.Common
{
    public interface ICreatedEntity
    {
        string CreatedBy { get; set; }

        DateTimeOffset CreatedOn { get; set; }
    }
}

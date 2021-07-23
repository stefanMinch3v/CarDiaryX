using System.Collections.Generic;
using System.Linq;

namespace CarDiaryX.Application.Common
{
    public class ErrorResult
    {
        public ErrorResult(IEnumerable<string> errors)
        {
            this.Errors = errors.ToArray();
        }

        public IReadOnlyCollection<string> Errors { get; }
    }
}

using System.Collections.Generic;

namespace CarDiaryX.Domain.Common
{
    public class PagingModel<T> where T : class
    {
        public PagingModel(IReadOnlyCollection<T> collection, int totalCount)
        {
            this.Collection = collection;
            this.TotalCount = totalCount;
        }

        public IReadOnlyCollection<T> Collection { get; }
        public int TotalCount { get; }
    }
}

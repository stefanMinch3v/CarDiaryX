using System;
using System.Collections.Generic;

namespace CarDiaryX.Application.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> mapFunction)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (mapFunction is null)
            {
                throw new ArgumentNullException(nameof(mapFunction));
            }

            foreach (var item in items)
            {
                mapFunction(item);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(
            this IEnumerable<T> source,
            int seed)
        {
            ArgumentNullException.ThrowIfNull(source);

            var random = new Random(seed);
            var items = source.ToArray();

            for (int i = items.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                (items[i], items[j]) = (items[j], items[i]);
            }

            return items;
        }
    }
}

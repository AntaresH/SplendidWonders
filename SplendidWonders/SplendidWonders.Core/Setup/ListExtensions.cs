using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public static class ListExtensions
    {
        public static IReadOnlyList<T> Draw<T>(this IReadOnlyList<T> list, int count)
        {
            var result = list.ToList().GetRange(0, count);
            list.ToList().RemoveRange(0, count);
            return result;
        }

        public static List<T> Draw<T>(this List<T> list, int count)
        {
            var result = list.GetRange(0, count);
            list.RemoveRange(0, count);
            return result;
        }
    }
}

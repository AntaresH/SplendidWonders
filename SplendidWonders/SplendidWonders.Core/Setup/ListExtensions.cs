using System;
using System.Collections.Generic;
using System.Text;

namespace SplendidWonders.Core.Setup
{
    public static class ListExtensions
    {
        public static List<T> Draw<T>(this List<T> list, int count)
        {
            var result = list.GetRange(0, Math.Min(count, list.Count));
            list.RemoveRange(0, Math.Min(count, list.Count));
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> helper, Action<T> action)
        {
            foreach (T item in helper)
            {
                action(item);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> helper)
        {
            return helper.Count() == 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(
            this IEnumerable<T> helper, 
            Action<T> action)
        {
            foreach (T item in helper)
            {
                action(item);
            }
        }

        public static T GetByType<T>(this IEnumerable<T> helper)
        {
            return helper.OfType<T>().SingleOrDefault();
        }

        public static T GetOrNew<T>(this ICollection<T> helper)
        {
            var item = helper.OfType<T>().FirstOrDefault();
            if (item == null)
            {
                item = Activator.CreateInstance<T>();
                helper.Add(item);
            }
            return item;
        }
    }
}

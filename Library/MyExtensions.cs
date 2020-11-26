using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartshopping.Library
{
    public static class MyExtensions
    {
        public static IEnumerable<T> SetValue<T>(this IEnumerable<T> items, Action<T> updateMethod)
        {
            var enumerable = items as T[] ?? items.ToArray();
            foreach (var item in enumerable)
            {
                updateMethod(item);
            }

            return enumerable;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quartz.Util;

namespace Smartshopping.Library
{
    public static class MyExtensions
    {
        public static async Task ForEachAsync<T>(this IList<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
        public static IQueryable<T> SetValue<T>(this IQueryable<T> items, Action<T> updateMethod)
        {
            foreach (var item in items)
            {
                updateMethod(item);
            }

            return items;
        }

        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate
        )
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> OrderByIf<T>(this IQueryable<T> source, string propertyName,
            bool descending, bool anotherLevel = false)
        {
            var param = Expression.Parameter(typeof(T), string.Empty); // I don't care about some naming
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);
            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] {typeof(T), property.Type},
                source.Expression,
                Expression.Quote(sort));
            return source.Provider.CreateQuery<T>(call);
        }
        
        public static IQueryable<T> MultipleOrderByIf<T>(
            this IQueryable<T> source,
            bool condition,
            IDictionary<string, int> dic)
        {
            if (!condition) return source;
            var index = 0;
            foreach (var (key, value) in dic)
            {
                source = source.OrderByIf(key, value == 1, index > 0);
                index++;
            }
            return source;
        }
    }
}
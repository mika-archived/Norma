using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Norma.Eta.Extensions
{
    public static class CollectionExt
    {
        public static void AddIfNotExists<T>(this ICollection<T> obj, T item)
        {
            if (!obj.Contains(item))
                obj.Add(item);
        }

        public static void AddIfNotExists<T>(this IList<T> obj, IList<T> target, T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (obj.Contains(item))
                return;
            target.Add(item);
        }

        public static void AddIfNotExists<T>(this IEnumerable<T> obj, DbSet<T> db, T item, Func<T, bool> condition)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (obj.Any(condition.Invoke))
                return;
            db.Add(item);
        }

        public static void AddIfNotExists<T>(this DbSet<T> obj, T item, Func<T, bool> condition)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (obj.Any(condition.Invoke))
                return;
            obj.Add(item);
        }

        public static void RemoveIfExists<T>(this ICollection<T> obj, T item)
        {
            if (obj.Contains(item))
                obj.Remove(item);
        }
    }
}
using System.Collections.Generic;

namespace Norma.Eta.Extensions
{
    public static class CollectionExt
    {
        public static void AddIfNotExists<T>(this ICollection<T> obj, T item)
        {
            if (!obj.Contains(item))
                obj.Add(item);
        }

        public static void RemoveIfExists<T>(this ICollection<T> obj, T item)
        {
            if (obj.Contains(item))
                obj.Remove(item);
        }
    }
}
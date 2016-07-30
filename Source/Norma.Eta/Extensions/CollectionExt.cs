using System.Collections.ObjectModel;

namespace Norma.Eta.Extensions
{
    public static class CollectionExt
    {
        public static void AddIfNotExists<T>(this Collection<T> obj, T item)
        {
            if (!obj.Contains(item))
                obj.Add(item);
        }
    }
}
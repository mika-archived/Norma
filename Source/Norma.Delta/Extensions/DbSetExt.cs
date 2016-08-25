using System.Data.Entity;
using System.Linq;

namespace Norma.Delta.Extensions
{
    internal static class DbSetExt
    {
        public static void RemoveIfExists<T>(this DbSet<T> obj, T item) where T : class
        {
            if (item == null)
                return;
            if (obj.Contains(item))
                obj.Remove(item);
        }
    }
}
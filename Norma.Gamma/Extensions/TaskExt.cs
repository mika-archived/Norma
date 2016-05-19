using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Norma.Gamma.Extensions
{
    internal static class TaskExt
    {
        internal static ConfiguredTaskAwaitable<TResult> Stay<TResult>(this Task<TResult> task)
            => task.ConfigureAwait(false);
    }
}
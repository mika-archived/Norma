namespace Norma.Models
{
    internal abstract class Library
    {
        public abstract string Name { get; }

        public abstract string Url { get; }

        public abstract string License { get; }
    }
}
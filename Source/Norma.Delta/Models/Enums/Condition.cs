namespace Norma.Delta.Models.Enums
{
    public enum Condition
    {
        /// <summary>
        ///     完全一致
        /// </summary>
        PerfectMatching,

        /// <summary>
        ///     部分一致
        /// </summary>
        PartialMatching,

        /// <summary>
        ///     前方一致
        /// </summary>
        Prefix,

        /// <summary>
        ///     後方一致
        /// </summary>
        Postfix
    }
}
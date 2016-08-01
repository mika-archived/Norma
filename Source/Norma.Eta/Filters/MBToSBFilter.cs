using Microsoft.VisualBasic;

namespace Norma.Eta.Filters
{
    // ReSharper disable once InconsistentNaming
    internal class MBToSBFilter : IFilter
    {
        #region Implementation of IFilter

        /// <summary>
        ///     全角文字を半角文字へ正規化します。
        /// </summary>
        /// <param name="str">e.g. "なもり／コミック百合姫にて連載中（一迅社刊）"</param>
        /// <returns>e.g. "なもり/コミック百合姫にて連載中(一迅社刊)"</returns>
        public string Call(string str)
        {
            return Strings.StrConv(str, VbStrConv.Narrow, 0x0411);
        }

        #endregion
    }
}
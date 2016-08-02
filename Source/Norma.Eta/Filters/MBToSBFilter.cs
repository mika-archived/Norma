using System.Text.RegularExpressions;

using Microsoft.VisualBasic;

namespace Norma.Eta.Filters
{
    // ReSharper disable once InconsistentNaming
    internal class MBToSBFilter : IFilter
    {
        private readonly Regex _regex = new Regex(@"[\uFF01-\uFF5E]+", RegexOptions.Compiled);

        #region Implementation of IFilter

        /// <summary>
        ///     全角文字を半角文字へ変換します。
        ///     対象は英数記号のみです。
        /// </summary>
        /// <param name="str">e.g. "なもり／コミック百合姫にて連載中（一迅社刊）"</param>
        /// <returns>e.g. "なもり/コミック百合姫にて連載中(一迅社刊)"</returns>
        public string Call(string str)
        {
            return _regex.Replace(str, w => Strings.StrConv(w.Value, VbStrConv.Narrow, 0x0411));
        }

        #endregion
    }
}
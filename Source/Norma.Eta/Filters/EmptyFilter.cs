using System.Collections.Generic;
using System.Linq;

namespace Norma.Eta.Filters
{
    public class EmptyFilter : IFilter
    {
        private readonly List<string> _emptyWords = new List<string>
        {
            "なし",
            "無し",
            "-",
            "n/a",
            "ｰ",
            "_"
        };

        #region Implementation of IFilter

        /// <summary>
        ///     出演者なしを正規化します。
        /// </summary>
        /// <param name="str">e.g. "無し", "-"</param>
        /// <returns>e.g. "", ""</returns>
        public string Call(string str)
        {
            return _emptyWords.Any(w => w == str) ? "" : str;
        }

        #endregion
    }
}
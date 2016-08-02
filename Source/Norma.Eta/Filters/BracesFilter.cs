using System.Text.RegularExpressions;

namespace Norma.Eta.Filters
{
    public class BracesFilter : IFilter
    {
        private readonly Regex _regex = new Regex(@"[\(〈《【「『\[\{].*?[\)〉》】」』\]\}]", RegexOptions.Compiled);

        #region Implementation of IFilter

        /// <summary>
        ///     カッコ以下を取り除きます。
        /// </summary>
        /// <param name="str">e.g. "しずちゃん(南海キャンディーズ)"</param>
        /// <returns>e.g. "しずちゃん"</returns>
        public string Call(string str)
        {
            return _regex.Replace(str, "");
        }

        #endregion
    }
}
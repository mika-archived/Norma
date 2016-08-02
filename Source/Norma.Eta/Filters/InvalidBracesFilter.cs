using System.Text.RegularExpressions;

namespace Norma.Eta.Filters
{
    public class InvalidBracesFilter : IFilter
    {
        private readonly Regex _regex = new Regex(@"\(.*?\)", RegexOptions.Compiled);

        #region Implementation of IFilter

        /// <summary>
        ///     カッコの一致を判定し、不正なものは除去します。
        /// </summary>
        /// <param name="str">e.g. "4 4位)山田浩之(−13", "9 8位)"</param>
        /// <returns>e.g. "", ""</returns>
        public string Call(string str)
        {
            if (!str.Contains("(") && !str.Contains(")"))
                return str;
            return !_regex.IsMatch(str) ? "" : str;
        }

        #endregion
    }
}
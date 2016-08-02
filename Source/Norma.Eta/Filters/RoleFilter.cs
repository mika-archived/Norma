using System.Text.RegularExpressions;

namespace Norma.Eta.Filters
{
    public class RoleFilter : IFilter
    {
        private readonly Regex _regex = new Regex("【.*?】", RegexOptions.Compiled);

        #region Implementation of IFilter

        /// <summary>
        ///     キャスト名を正規化します。
        /// </summary>
        /// <param name="str">e.g. "インデックス: 井口裕香", "【コメンテーター】"</param>
        /// <returns>e.g. "井口裕香", ""</returns>
        public string Call(string str)
        {
            if (str.Split(':').Length > 1)
                str = str.Split(':')[1];
            str = _regex.Replace(str, "");
            return str;
        }

        #endregion
    }
}
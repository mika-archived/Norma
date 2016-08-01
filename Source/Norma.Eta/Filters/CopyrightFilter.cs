namespace Norma.Eta.Filters
{
    internal class CopyrightFilter : IFilter
    {
        #region Implementation of IFilter

        /// <summary>
        ///     コピーライト表記を正規化します。
        /// </summary>
        /// <param name="str">e.g. "(C)2016 水瀬るるう・芳文社／製作委員会は思春期", "©テレビ朝日"</param>
        /// <returns>e.g. "(c)2016 水瀬るるう・芳文社／製作委員会は思春期", "(c)テレビ朝日"</returns>
        public string Call(string str)
        {
            return str.Replace("(C)", "©").Replace("(c)", "©");
        }

        #endregion
    }
}
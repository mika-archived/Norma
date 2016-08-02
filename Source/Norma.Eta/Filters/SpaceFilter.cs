namespace Norma.Eta.Filters
{
    public class SpaceFilter : IFilter
    {
        #region Implementation of IFilter

        /// <summary>
        ///     スペースを取り除きます。
        /// </summary>
        /// <param name="str">e.g. " Cyberagent", " 小峠 "</param>
        /// <returns>e.g. "Cyberagent", "小峠"</returns>
        public string Call(string str)
        {
            return str.Trim();
        }

        #endregion
    }
}
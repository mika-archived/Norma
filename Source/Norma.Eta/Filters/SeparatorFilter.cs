namespace Norma.Eta.Filters
{
    public class SeparatorFilter : IFilter
    {
        #region Implementation of IFilter

        /// <summary>
        ///     セパレータを正規化します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Call(string str)
        {
            return str;
        }

        #endregion
    }
}
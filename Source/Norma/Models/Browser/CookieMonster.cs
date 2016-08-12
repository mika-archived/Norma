using CefSharp;

namespace Norma.Models.Browser
{
    internal class CookieMonster : ICookieVisitor
    {
        #region Implementation of IDisposable

        public void Dispose()
        {
            //
        }

        public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            deleteCookie = true;
            return true;
        }

        #endregion
    }
}
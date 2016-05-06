using CefSharp.Wpf;

using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel
    {
        public IWpfWebBrowser WebBrowser { get; set; }

        public AbemaHostViewModel()
        {
            Address = "https://abema.tv/";
        }

        #region Address

        private string _address;

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        #endregion
    }
}
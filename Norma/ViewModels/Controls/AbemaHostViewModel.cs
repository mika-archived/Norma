using CefSharp.Wpf;

using Norma.Extensions;
using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel
    {
        private readonly ShellViewModel _parentViewModel;
        private OnAirPageJavaScriptHost _javaScritHost;

        public AbemaCommentViewModel CommentViewModel { get; }

        public AbemaHostViewModel(ShellViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            CommentViewModel = new AbemaCommentViewModel();
            Address = "https://abema.tv/";
        }

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
            _javaScritHost = new OnAirPageJavaScriptHost(WebBrowser).AddTo(this);
            _javaScritHost.Subscribe(nameof(_javaScritHost.Title), w => _parentViewModel.Title = _javaScritHost.Title);
        }

        #region WebBrowser

        private IWpfWebBrowser _webBrowser;

        public IWpfWebBrowser WebBrowser
        {
            get { return _webBrowser; }
            set
            {
                if (SetProperty(ref _webBrowser, value))
                    WebBrowserInitialized();
            }
        }

        #endregion

        #region Address

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                if (SetProperty(ref _address, value) && _javaScritHost != null)
                    _javaScritHost.Address = value;
            }
        }

        #endregion
    }
}
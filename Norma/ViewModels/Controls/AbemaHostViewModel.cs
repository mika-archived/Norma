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
        private JavaScriptHost _javaScritHost;

        public AbemaCommentViewModel CommentViewModel { get; }
        public AbemaProgramInfoViewModel ProgramInfoViewModel { get; }

        public AbemaHostViewModel(ShellViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            CommentViewModel = new AbemaCommentViewModel(this);
            ProgramInfoViewModel = new AbemaProgramInfoViewModel(this);
            Address = $"https://abema.tv/now-on-air/{Configuration.Instance.Root.LastViewedChannel.ToUrlString()}";
        }

        private void WebBrowserInitialized()
        {
            if (WebBrowser == null)
                return;
            _javaScritHost = new JavaScriptHost(WebBrowser).AddTo(this);
            _javaScritHost.Subscribe(nameof(_javaScritHost.Title), w =>
            {
                _parentViewModel.Title = _javaScritHost.Title;
                CommentViewModel.OnProgramChanged(_javaScritHost.RawTitle);
            });
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
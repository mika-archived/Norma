using CefSharp.Wpf;

using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AbemaHostViewModel : ViewModel
    {
        public IWpfWebBrowser WebBrowser { get; set; }
    }
}
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Norma.Eta;
using Norma.Eta.Models.Configurations;
using Norma.Eta.Mvvm;

using Prism.Commands;

using Reactive.Bindings;

namespace Norma.ViewModels.Tabs.Options
{
    internal class BrowserViewModel : ViewModel
    {
        public ReactiveProperty<bool> DisableChangeChannelByMouseWheel { get; private set; }
        public ReactiveProperty<bool> ReloadPageOnBroadcastCommercials { get; private set; }
        public ReactiveProperty<string> CustomCss { get; private set; }

        public BrowserViewModel(BrowserConfig bc)
        {
            DisableChangeChannelByMouseWheel = ReactiveProperty.FromObject(bc, w => w.DisableChangeChannelByMouseWheel)
                                                               .AddTo(this);
            ReloadPageOnBroadcastCommercials = ReactiveProperty.FromObject(bc, w => w.ReloadPageOnBroadcastCommercials)
                                                               .AddTo(this);
            CustomCss = ReactiveProperty.FromObject(bc, w => w.CustomCss).AddTo(this);
        }

        #region DeleteBrowserCacheCommand

        private ICommand _deleteBrowserCacheCommand;

        public ICommand DeleteBrowserCacheCommand
            => _deleteBrowserCacheCommand ?? (_deleteBrowserCacheCommand = new DelegateCommand(DeleteBrowserCache));

        private void DeleteBrowserCache()
        {
            Task.Run(() =>
            {
                // Model なり Service なりでやったほうがいい
                foreach (var file in Directory.GetFiles(NormaConstants.CefCacheDir, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            });
        }

        #endregion

        #region DeleteBrowserCookiesCommand

        private ICommand _deleteBrowserCookieCommand;

        public ICommand DeleteBrowserCookieCommand
            => _deleteBrowserCookieCommand ?? (_deleteBrowserCookieCommand = new DelegateCommand(DeleteBrowserCookie));

        private void DeleteBrowserCookie()
        {
            // TODO: Delete Beowser Cookies
        }

        #endregion
    }
}
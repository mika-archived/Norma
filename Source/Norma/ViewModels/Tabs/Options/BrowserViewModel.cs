using System.Windows.Input;

using Norma.Eta.Models.Configurations;
using Norma.Eta.Mvvm;

using Prism.Commands;

using Reactive.Bindings;

namespace Norma.ViewModels.Tabs.Options
{
    internal class BrowserViewModel : ViewModel
    {
        public ReactiveProperty<bool> HiddenHeaderControls { get; private set; }
        public ReactiveProperty<bool> HiddenFooterControls { get; private set; }
        public ReactiveProperty<bool> HiddenSideControls { get; private set; }
        public ReactiveProperty<bool> HiddenLeftControls { get; private set; }
        public ReactiveProperty<bool> DisableChangeChannelByMouseWheel { get; private set; }
        public ReactiveProperty<bool> ReloadPageOnBroadcastCommercials { get; private set; }

        public BrowserViewModel(BrowserConfig bc)
        {
            HiddenHeaderControls = ReactiveProperty.FromObject(bc, w => w.HiddenHeaderControls).AddTo(this);
            HiddenFooterControls = ReactiveProperty.FromObject(bc, w => w.HiddenFooterControls).AddTo(this);
            HiddenSideControls = ReactiveProperty.FromObject(bc, w => w.HiddenSideControls).AddTo(this);
            HiddenSideControls = ReactiveProperty.FromObject(bc, w => w.HiddenSideControls).AddTo(this);
            DisableChangeChannelByMouseWheel = ReactiveProperty.FromObject(bc, w => w.DisableChangeChannelByMouseWheel)
                                                               .AddTo(this);
            ReloadPageOnBroadcastCommercials = ReactiveProperty.FromObject(bc, w => w.ReloadPageOnBroadcastCommercials)
                                                               .AddTo(this);
        }

        #region DeleteBrowserCacheCommand

        private ICommand _deleteBrowserCacheCommand;

        public ICommand DeleteBrowserCacheCommand
            => _deleteBrowserCacheCommand ?? (_deleteBrowserCacheCommand = new DelegateCommand(DeleteBrowserCache));

        private void DeleteBrowserCache()
        {
            // TODO: Delete Browser Caches
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
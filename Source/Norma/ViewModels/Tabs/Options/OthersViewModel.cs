using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Models.Configurations;
using Norma.Eta.Models.Enums;
using Norma.Eta.Mvvm;
using Norma.Eta.Properties;

using Prism.Commands;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Tabs.Options
{
    internal class OthersViewModel : ViewModel
    {
        public ReactiveProperty<UITheme> SelectedTheme { get; private set; }

        public OthersViewModel(OthersConfig oc)
        {
            SelectedTheme = oc.ToReactivePropertyAsSynchronized(w => w.Theme).AddTo(this);
            RegStartupButtonContent = AppStartup.IsRegistered ? Resources.UnregFromStartup : Resources.RegToStartup;
        }

        #region RegisterStartupCommand

        private ICommand _registerStartupCommand;

        public ICommand RegisterStartupCommand
            => _registerStartupCommand ?? (_registerStartupCommand = new DelegateCommand(RegisterStartup));

        private void RegisterStartup()
        {
            if (AppStartup.IsRegistered)
                AppStartup.Unregister();
            else
                AppStartup.Register();
            RegStartupButtonContent = AppStartup.IsRegistered ? Resources.UnregFromStartup : Resources.RegToStartup;
        }

        #endregion

        #region RegStartupButtonContent

        private string _regStartupButtonContent;

        public string RegStartupButtonContent
        {
            get { return _regStartupButtonContent; }
            set { SetProperty(ref _regStartupButtonContent, value); }
        }

        #endregion
    }
}
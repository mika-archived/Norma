using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Views;

using Prism.Unity;

namespace Norma
{
    internal class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of Bootstrapper

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => Application.Current.MainWindow.Show();

        #endregion
    }
}
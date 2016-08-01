using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Iota.Views;

using Prism.Unity;

namespace Norma.Iota
{
    internal class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<StatusService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<DbConnection>(new ContainerControlledLifetimeManager());
            Container.RegisterType<Configuration>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaApiClient>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        #endregion
    }
}
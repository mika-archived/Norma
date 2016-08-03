using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Delta.Services;
using Norma.Eta.Models;
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

            Container.RegisterType<DatabaseService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ReservationService>(new ContainerControlledLifetimeManager());
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
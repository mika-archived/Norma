using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Models;
using Norma.Views;

using Prism.Unity;

namespace Norma
{
    internal class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<StatusService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<DatabaseService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ReservationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<TimetableService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<Configuration>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaApiClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<NetworkHandler>(new ContainerControlledLifetimeManager());

            AppInitializer.Initialize();
        }

        #endregion

        #region Overrides of Bootstrapper

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell()
        {
            AppInitializer.PostInitialize();

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        #endregion
    }
}
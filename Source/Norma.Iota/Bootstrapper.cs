using System.Windows;

using Microsoft.Practices.ServiceLocation;
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

            Container.RegisterType<DatabaseService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ReservationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<Configuration>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaApiClient>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
        {
            var timetableService = ServiceLocator.Current.GetInstance<TimetableService>();
            timetableService.Initialize();

            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        #endregion
    }
}
using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Eta.Models;
using Norma.Ipsilon.Views;

using Prism.Unity;

namespace Norma.Ipsilon
{
    internal class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            AppInitializer.Initialize();

            Container.RegisterInstance(AppInitializer.Configuration, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.AbemaApiHost, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.Timetable, new ContainerControlledLifetimeManager());
            Container.RegisterType(typeof(Reservation), new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => AppInitializer.PostInitialize();

        #endregion
    }
}
using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Delta.Services;
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

            Container.RegisterType<DbConnection>(new ContainerControlledLifetimeManager());

            Container.RegisterInstance(AppInitializer.Configuration, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.AbemaApiHost, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.Timetable, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.ConnectOps, new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => AppInitializer.PostInitialize();

        #endregion
    }
}
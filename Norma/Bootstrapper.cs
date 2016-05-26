using System.Windows;

using Microsoft.Practices.Unity;

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

            AppInitializer.Initialize();

            Container.RegisterInstance(AppInitializer.Configuration, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.AbemaApiHost, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.Timetable, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(AppInitializer.AbemaState, new ContainerControlledLifetimeManager());
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
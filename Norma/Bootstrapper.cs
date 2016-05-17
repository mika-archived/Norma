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
            Container.RegisterType<Configuration>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaApiHost>(new ContainerControlledLifetimeManager());
            Container.RegisterType<Timetable>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaState>(new ContainerControlledLifetimeManager());
        }

        #endregion

        #region Overrides of Bootstrapper

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => Application.Current.MainWindow.Show();

        #endregion
    }
}
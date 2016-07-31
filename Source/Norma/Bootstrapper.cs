using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Models;
using Norma.Views;

using Prism.Unity;

namespace Norma
{
    internal class Bootstrapper : UnityBootstrapper, IServiceLocator
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
        }

        protected override void ConfigureServiceLocator() => ServiceLocator.SetLocatorProvider(() => this);

        #endregion

        #region Overrides of Bootstrapper

        protected override DependencyObject CreateShell()
        {
            AppInitializer.Initialize();
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            AppInitializer.PostInitialize();

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        #endregion

        // -------------------
        // あまりよくなさそう。

        #region Implementation of IServiceLocator

        public object GetService(Type serviceType) => Container.Resolve(serviceType);

        public object GetInstance(Type serviceType) => Container.Resolve(serviceType);

        public object GetInstance(Type serviceType, string key) => Container.Resolve(serviceType, key);

        public IEnumerable<object> GetAllInstances(Type serviceType) => Container.ResolveAll(serviceType);

        public TService GetInstance<TService>() => Container.Resolve<TService>();

        public TService GetInstance<TService>(string key) => Container.Resolve<TService>(key);

        public IEnumerable<TService> GetAllInstances<TService>() => Container.ResolveAll<TService>();

        #endregion
    }
}
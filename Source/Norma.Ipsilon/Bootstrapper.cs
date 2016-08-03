using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using Norma.Delta.Services;
using Norma.Eta.Models;
using Norma.Eta.Services;
using Norma.Ipsilon.Views;

using Prism.Unity;

namespace Norma.Ipsilon
{
    internal class Bootstrapper : UnityBootstrapper, IServiceLocator
    {
        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            AppInitializer.Initialize();

            Container.RegisterType<DatabaseService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ReservationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<TimetableService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<Configuration>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AbemaApiClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ConnectOps>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => AppInitializer.PostInitialize();

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
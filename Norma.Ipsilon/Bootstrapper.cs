using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Eta.Models;
using Norma.Ipsilon.Views;

using Prism.Unity;

namespace Norma.Ipsilon
{
    internal class Bootstrapper : UnityBootstrapper
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly Configuration _configuration;
        private readonly Timetable _timetable;

        public Bootstrapper()
        {
            _configuration = new Configuration();
            _abemaApiHost = new AbemaApiHost(_configuration);
            _timetable = new Timetable(_abemaApiHost);
        }

        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            _abemaApiHost.Initialize();
            _timetable.Sync();

            Container.RegisterInstance(_configuration, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(_abemaApiHost, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(_timetable, new ContainerControlledLifetimeManager());
            Container.RegisterType(typeof(Reservation), new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell()
        {
            // Nothing to do
        }

        #endregion
    }
}
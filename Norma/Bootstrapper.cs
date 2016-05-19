using System.Windows;

using Microsoft.Practices.Unity;

using Norma.Models;
using Norma.Views;

using Prism.Unity;

namespace Norma
{
    internal class Bootstrapper : UnityBootstrapper
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        private readonly Timetable _timetable;

        public Bootstrapper()
        {
            // どこで初期化すべき？
            _configuration = new Configuration();
            _abemaApiHost = new AbemaApiHost(_configuration);
            _timetable = new Timetable(_abemaApiHost);
            _abemaState = new AbemaState(_configuration, _timetable);
        }

        #region Overrides of UnityBootstrapper

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            _abemaApiHost.Initialize();
            _timetable.Sync();
            _abemaState.Start();

            Container.RegisterInstance(_configuration, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(_abemaApiHost, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(_timetable, new ContainerControlledLifetimeManager());
            Container.RegisterInstance(_abemaState, new ContainerControlledLifetimeManager());
        }

        #endregion

        #region Overrides of Bootstrapper

        protected override DependencyObject CreateShell() => Container.Resolve<Shell>();

        protected override void InitializeShell() => Application.Current.MainWindow.Show();

        #endregion
    }
}
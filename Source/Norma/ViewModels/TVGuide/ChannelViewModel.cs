using System.Reactive.Linq;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Prism.Commands;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.TVGuide
{
    internal class ChannelViewModel : ViewModel
    {
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        private readonly AbemaChannel _model;

        public string LogoUrl => _model.LogoUrl;
        public ReadOnlyReactiveProperty<string> Title { get; private set; }
        public ReadOnlyReactiveProperty<string> StartTime { get; private set; }
        public ReadOnlyReactiveProperty<string> EndTime { get; private set; }
        public ReadOnlyReactiveProperty<string> ThumbnailUrl { get; private set; }

        public ChannelViewModel(AbemaState abemaState, AbemaChannel channel, Configuration configuration)
        {
            _abemaState = abemaState;
            _model = channel;
            _configuration = configuration;
            Title = _model.ObserveProperty(w => w.Title)
                          .ToReadOnlyReactiveProperty()
                          .AddTo(this);
            StartTime = _model.ObserveProperty(w => w.StartAt)
                              .Select(w => w.ToString("HH:mm"))
                              .ToReadOnlyReactiveProperty()
                              .AddTo(this);
            EndTime = _model.ObserveProperty(w => w.EndAt)
                            .Select(w => w.ToString("HH:mm"))
                            .ToReadOnlyReactiveProperty()
                            .AddTo(this);
            ThumbnailUrl = _model.ObserveProperty(x => x.ThumbnailUrl).ToReadOnlyReactiveProperty().AddTo(this);
        }

        // CallMethodAction
        public void ChannelClick() => _abemaState.CurrentChannel = _model.Channel;

        #region AddToFavoriteCommand

        private DelegateCommand _addToFavoriteCommand;

        public DelegateCommand AddToFavoriteCommand
            => _addToFavoriteCommand ?? (_addToFavoriteCommand = new DelegateCommand(AddToFavorite, CanAddToFavorite));

        private void AddToFavorite()
        {
            _configuration.Root.Internal.FavoriteChannels.Add(_model.Channel.ChannelId);
            AddToFavoriteCommand.RaiseCanExecuteChanged();
            DeleteFromFavoriteCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddToFavorite() => !_configuration.Root.Internal.FavoriteChannels.Contains(_model.Channel.ChannelId);

        #endregion

        #region DeleteFromFavoriteCommand

        private DelegateCommand _deleteFromFavoriteCommand;

        public DelegateCommand DeleteFromFavoriteCommand
            => _deleteFromFavoriteCommand ?? (_deleteFromFavoriteCommand = new DelegateCommand(DeleteFromFavorite, CanDeleteFromFavorite));

        private void DeleteFromFavorite()
        {
            _configuration.Root.Internal.FavoriteChannels.Remove(_model.Channel.ChannelId);
            AddToFavoriteCommand.RaiseCanExecuteChanged();
            DeleteFromFavoriteCommand.RaiseCanExecuteChanged();
        }

        private bool CanDeleteFromFavorite() => _configuration.Root.Internal.FavoriteChannels.Contains(_model.Channel.ChannelId);

        #endregion
    }
}
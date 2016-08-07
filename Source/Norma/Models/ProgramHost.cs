using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Norma.Delta.Models;
using Norma.Eta.Properties;
using Norma.Eta.Services;

using Prism.Mvvm;

using Reactive.Bindings.Extensions;

namespace Norma.Models
{
    internal class ProgramHost : BindableBase, IDisposable
    {
        private readonly AbemaState _abemaState;
        private readonly CompositeDisposable _compositeDisposable;
        private readonly object _lockObj = new object();
        private readonly StatusService _statusService;

        public ObservableCollection<string> Casts { get; }

        public ObservableCollection<string> Crews { get; }

        public ProgramHost(AbemaState abemaState, StatusService statusService)
        {
            Casts = new ObservableCollection<string>();
            Crews = new ObservableCollection<string>();
            _compositeDisposable = new CompositeDisposable();

            _abemaState = abemaState;
            _statusService = statusService;
            _compositeDisposable.Add(abemaState.ObserveProperty(w => w.CurrentEpisode)
                                               .Where(w => w != null)
                                               .Subscribe(w => FetchProgramInfo()));
            FetchProgramInfo(); // Init
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

        #endregion

        private void FetchProgramInfo()
        {
            lock (_lockObj)
            {
                _statusService.UpdateStatus(Resources.FetchingProgramInformation);
                var slot = _abemaState.CurrentSlot;
                var episode = _abemaState.CurrentEpisode;

                if (slot == null)
                {
                    Title = "";
                    return;
                }

                Casts.Clear();
                Crews.Clear();

                Title = slot.Episodes.Count == 1 ? slot.Title : $"{slot.Title} - #{episode.Sequence}";
                Description = slot.Description;
                episode.Casts.ForEach(w => Casts.Add(w.Name));
                episode.Crews.ForEach(w => Crews.Add(w.Name));

                ProvideThumbnails(episode);

                _statusService.UpdateStatus(Resources.FetchedProgramInformation);
            }
        }

        private void ProvideThumbnails(Episode episode)
        {
            var scenes = episode.Thumbnails;

            // Init
            if (scenes.Count > 0)
            {
                Thumbnail1 = $"https://hayabusa.io/abema/programs/{episode.EpisodeId}/{scenes.Skip(0).First().Path}.w135.png";
                Thumbnail2 = scenes.Count >= 2
                    ? $"https://hayabusa.io/abema/programs/{episode.EpisodeId}/{scenes.Skip(1).First().Path}.w135.png"
                    : "";
            }
            else
                Thumbnail1 = Thumbnail2 = "";
        }

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            private set { SetProperty(ref _description, value); }
        }

        #endregion

        #region Thumbnail1

        private string _thumbnail1;

        public string Thumbnail1
        {
            get { return _thumbnail1; }
            private set { SetProperty(ref _thumbnail1, value); }
        }

        #endregion

        #region Thumbnail2

        private string _thumbnail2;

        public string Thumbnail2
        {
            get { return _thumbnail2; }
            private set { SetProperty(ref _thumbnail2, value); }
        }

        #endregion
    }
}
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using Microsoft.Practices.ObjectBuilder2;

using Norma.Eta.Properties;
using Norma.Gamma.Models;

using Prism.Mvvm;

using Reactive.Bindings.Extensions;

namespace Norma.Models
{
    internal class ProgramHost : BindableBase, IDisposable
    {
        private readonly AbemaState _abemaState;
        private readonly CompositeDisposable _compositeDisposable;

        public ProgramHost(AbemaState abemaState)
        {
            Casts = new ObservableCollection<string>();
            Crews = new ObservableCollection<string>();
            _compositeDisposable = new CompositeDisposable();

            _abemaState = abemaState;
            _compositeDisposable.Add(abemaState.ObserveProperty(w => w.CurrentProgram)
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
            StatusInfo.Instance.Text = Resources.FetchingProgramInformation;
            var slot = _abemaState.CurrentSlot;
            var program = _abemaState.CurrentProgram;
            if (slot == null)
            {
                Title = "";
                return;
            }
            // ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
            if (false)
            {
                Title = slot.Title;
                Description = slot.Programs[0].Episode.Overview;
                ProvideCredits(slot.Programs[0].Credit);
                ProvideThumbnails(slot.Programs[0]);
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                Title = $"{slot.Title} - {program.Episode.Name} \"{program.Episode.Title}\"";
                Description = program.Episode.Overview;
                ProvideCredits(program.Credit);
                ProvideThumbnails(program);
            }
            StatusInfo.Instance.Text = Resources.FetchedProgramInformation;
        }

        private void ProvideCredits(Credit credit)
        {
            Casts.Clear();
            if (credit.Cast?.Length > 0)
            {
                credit.Cast?.ForEach(w => Casts.Add(w));
                HasCasts = true;
            }
            else
                HasCasts = false;

            Crews.Clear();
            if (credit.Crews?.Length > 0)
            {
                credit.Crews?.ForEach(w => Crews.Add(w));
                HasCrews = true;
            }
            else
                HasCrews = false;
        }

        private void ProvideThumbnails(Program program)
        {
            var scenes = program.ProvidedInfo.SceneThumbImgs;

            // Init
            if (scenes?.Length > 0)
            {
                Thumbnail1 = $"https://hayabusa.io/abema/programs/{program.Id}/{scenes[0]}.w135.png";
                Thumbnail2 = scenes.Length >= 2
                    ? $"https://hayabusa.io/abema/programs/{program.Id}/{scenes[1]}.w135.png"
                    : "";
                return;
            }
            Thumbnail2 = "";
            Thumbnail1 = !string.IsNullOrWhiteSpace(program.ProvidedInfo.ThumbImg)
                ? $"https://hayabusa.io/abema/programs/{program.Id}/{program.ProvidedInfo.ThumbImg}.w135.png"
                : "";
        }

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Description

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        #endregion

        #region Thumbnail1

        private string _thumbnail1;

        public string Thumbnail1
        {
            get { return _thumbnail1; }
            set { SetProperty(ref _thumbnail1, value); }
        }

        #endregion

        #region Thumbnail2

        private string _thumbnail2;

        public string Thumbnail2
        {
            get { return _thumbnail2; }
            set { SetProperty(ref _thumbnail2, value); }
        }

        #endregion

        public ObservableCollection<string> Casts { get; }

        public ObservableCollection<string> Crews { get; }

        #region HasCasts

        private bool _hasCasts;

        public bool HasCasts
        {
            get { return _hasCasts; }
            set { SetProperty(ref _hasCasts, value); }
        }

        #endregion

        #region HasCrews

        private bool _hasCrews;

        public bool HasCrews
        {
            get { return _hasCrews; }
            set { SetProperty(ref _hasCrews, value); }
        }

        #endregion
    }
}
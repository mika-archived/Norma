using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Microsoft.Practices.ObjectBuilder2;

using Norma.Gamma.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class ProgramHost : BindableBase, IDisposable
    {
        private AbemaChannel _channel;
        private IDisposable _disposable;
        private Slot _oldSlot;

        public ProgramHost()
        {
            Casts = new ObservableCollection<string>();
            Crews = new ObservableCollection<string>();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        #endregion

        public void OnChannelChanged(AbemaChannel channel)
        {
            _channel = channel;
            _disposable?.Dispose();

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                                    .Subscribe(w => FetchProgramInfo());
        }

        private void FetchProgramInfo()
        {
            StatusInfo.Instance.Text = "Fetching program information.";
            var ts = Timetable.Instance.Media;
            var currenSchedule = ts.ChannelSchedules.First(w => w.ChannelId == _channel.ToUrlString()); // 今日
            var currentProgram = currenSchedule.Slots.Single(w => w.StartAt <= DateTime.Now && w.EndAt >= DateTime.Now);
            if (_oldSlot != null && currentProgram.Id == _oldSlot.Id)
                return;

            _oldSlot = currentProgram;
            // 番組名 or プログラム名
            // ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
            if (false)
            {
                Title = currentProgram.Title;
                Description = currentProgram.Programs[0].Episode.Overview;
                ProvideCredits(currentProgram.Programs[0].Credit);
                ProvideThumbnails(currentProgram.Programs[0]);
                return;
            }
            var perTime = (currentProgram.EndAt - currentProgram.StartAt).TotalSeconds /
                          currentProgram.Programs.Length;
            var fill = 0;
            while (!(currentProgram.StartAt.AddSeconds(perTime * fill) <= DateTime.Now &&
                     DateTime.Now <= currentProgram.StartAt.AddSeconds(perTime * ++fill))) {}
            fill--;
            var program = currentProgram.Programs[fill];
            Title = $"{currentProgram.Highlight} - {program.Episode.Name} \"{program.Episode.Title}\"";
            Description = program.Episode.Overview;
            ProvideCredits(program.Credit);
            ProvideThumbnails(program);
            StatusInfo.Instance.Text = "Fetched program information.";
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
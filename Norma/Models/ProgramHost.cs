using System;
using System.Linq;
using System.Reactive.Linq;

using Norma.Gamma.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class ProgramHost : BindableBase, IDisposable
    {
        private AbemaChannel _channel;
        private IDisposable _disposable;

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
            // 番組名 or プログラム名
            // ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
            if (false)
            {
                Title = currentProgram.Title;
                Description = currentProgram.Programs[0].Episode.Overview;
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
            ProvideThumbnails(program);
            StatusInfo.Instance.Text = "Fetched program information.";
        }

        private void ProvideThumbnails(Program program)
        {
            var scenes = program.ProvidedInfo.SceneThumbImgs;

            // Init
            if (scenes.Length > 0)
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

        #region HasInfo

        private bool _hasInfo;

        public bool HasInfo
        {
            get { return _hasInfo; }
            set { SetProperty(ref _hasInfo, value); }
        }

        #endregion

        #region Title

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (SetProperty(ref _title, value))
                    HasInfo = !string.IsNullOrWhiteSpace(value);
            }
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
    }
}
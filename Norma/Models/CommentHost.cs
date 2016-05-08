using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Gamma.Models;

#pragma warning disable 0414

namespace Norma.Models
{
    internal class CommentHost : IDisposable
    {
        private AbemaChannels _channel;
        private IDisposable _disposable;
        private bool _isCm;
        private string _slotId;
        private string _title;
        public ObservableCollection<Comment> Comments { get; set; }

        public CommentHost()
        {
            Comments = new ObservableCollection<Comment>();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable.Dispose();
        }

        #endregion

        public void OnChannelChanged(AbemaChannels channel)
        {
            _channel = channel;
        }

        public void OnProgramChanged(string title)
        {
            if (title == "(CM)")
            {
                _isCm = true;
                return;
            }
            _isCm = false;
            if (_title == title)
                return;

            // 新しい SlotId に変更された。
            Comments.Clear();
            _title = title;
            _disposable?.Dispose();

            FetchProgram();
        }

        private void FetchProgram()
        {
            var ts = Timetable.Instance.Media;
            var currenSchedule = ts.ChannelSchedules.First(w => w.ChannelId == _channel.ToUrlString()); // 今日
            var currentProgram = currenSchedule.Slots.Single(w => w.StartAt <= DateTime.Now && w.EndAt >= DateTime.Now);
            _slotId = currentProgram.Id;
            SubscribeComment();
        }

        private void SubscribeComment()
        {
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                                    .SkipWhile(w => _isCm)
                                    .Subscribe(async w => await FetchComments());
        }

        private async Task FetchComments()
        {
            var comments = await AbemaApiHost.Instance.Comments(_slotId);
            foreach (var comment in comments.CommentList.OrderBy(w => w.CreatedAtMs))
            {
                if (Comments.Any(w => w.Id == comment.Id))
                    continue;
                Comments.Insert(0, comment);
            }
        }
    }
}
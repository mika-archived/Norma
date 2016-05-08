using System.Collections.ObjectModel;
using System.Diagnostics;

using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class CommentHost
    {
        private AbemaChannels _channel;
        private bool _isCM;
        private string _title;
        public ObservableCollection<Comment> Comments { get; set; }

        public CommentHost()
        {
            Comments = new ObservableCollection<Comment>();
        }

        public void OnChannelChanged(AbemaChannels channel)
        {
            _channel = channel;
        }

        public void OnProgramChanged(string title)
        {
            if (title == "(CM)")
            {
                _isCM = true;
                return;
            }
            _isCM = false;
            if (_title == title)
                return;

            // 新しい SlotId に変更された。
            Comments.Clear();
            _title = title;

            SubscribeComment();
        }

        private void SubscribeComment()
        {
            var ts = Timetable.Instance.Media;
            Debug.WriteLine("aaaa");
        }
    }
}
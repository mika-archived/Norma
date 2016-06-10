using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Norma.Eta.Extensions;
using Norma.Eta.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaChannels : BindableBase, IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly Timetable _timetable;
        public ObservableCollection<AbemaChannel> Channels { get; }

        public AbemaChannels(Timetable timetable)
        {
            _timetable = timetable;
            Channels = new ObservableCollection<AbemaChannel>();
            FetchChannels();
            _disposable = Observable.Timer(TimeSpanExt.OneSecond, TimeSpanExt.OneSecond).Subscribe(w => FetchChannels());
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable.Dispose();
        }

        #endregion

        private void FetchChannels()
        {
            var channels = _timetable.CurrentChannels();
            var temp = Channels.ToArray();
            // 削除
            foreach (var channel in temp)
            {
                if (!channels.Contains(channel))
                    Channels.Remove(channel);
            }
            // 追加
            foreach (var channel in channels.Select((v, i) => new {Value = v, Index = i}))
            {
                if (Channels.Contains(channel.Value))
                    continue;
                Channels.Insert(channel.Index, channel.Value);
            }
        }
    }
}
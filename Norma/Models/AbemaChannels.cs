using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Norma.Eta.Models;

using Prism.Mvvm;

namespace Norma.Models
{
    internal class AbemaChannels : BindableBase, IDisposable
    {
        private readonly IDisposable _disposable;
        public ObservableCollection<AbemaChannel> Channels { get; }

        public AbemaChannels()
        {
            Channels = new ObservableCollection<AbemaChannel>();
            var channels = (AbemaChannel[]) Enum.GetValues(typeof(AbemaChannel));
            foreach (var value in channels.Skip(1))
                Channels.Add(value);

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Subscribe(w => FetchChannels());
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable.Dispose();
        }

        #endregion

        private void FetchChannels()
        {
            // Mon ~ Thu の 00:00 ~ 02:30 限定で、 OnegaiRanking が追加されてる
            var today = DateTime.Now;
            var dayOfWeek = today.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Monday)
            {
                Remove();
                return;
            }
            if (today.Hour >= 3)
            {
                Remove();
                return;
            }
            if (today.Hour == 2 && today.Minute > 30)
            {
                Remove();
                return;
            }

            if (Channels.Contains(AbemaChannel.OnegaiRanking))
                return;
            Channels.Insert(0, AbemaChannel.OnegaiRanking);
        }

        private void Remove()
        {
            if (!Channels.Contains(AbemaChannel.OnegaiRanking))
                return;
            Channels.Remove(AbemaChannel.OnegaiRanking);
        }
    }
}
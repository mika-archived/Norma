using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Norma.Gamma.Models;
using Norma.Helpers;

using Prism.Mvvm;

#pragma warning disable 0414

namespace Norma.Models
{
    internal class CommentHost : BindableBase, IDisposable
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly AbemaState _abemaState;
        private readonly CompositeDisposable _compositeDisposable;
        private IDisposable _disposable; // for Comment synchronizer.

        public ObservableCollection<Comment> Comments { get; set; }

        public CommentHost(AbemaApiHost abemaApiHost, AbemaState abemaState)
        {
            Comments = new ObservableCollection<Comment>();
            _compositeDisposable = new CompositeDisposable();

            _abemaApiHost = abemaApiHost;
            _abemaState = abemaState;
            _compositeDisposable.Add(_abemaState.Subscribe(nameof(_abemaState.CurrentSlot), w => ReloadComments()));
            _compositeDisposable.Add(_abemaState.Subscribe(nameof(_abemaState.IsBroadcastCm), w => StopFetchComment()));
            ReloadComments();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        #endregion

        private void ReloadComments()
        {
            _disposable?.Dispose();
            Comments.Clear();

            if (_abemaState.CurrentSlot == null)
                return;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                                    .Subscribe(async w => await FetchComment());
        }

        private void StopFetchComment()
        {
            if (_abemaState.IsBroadcastCm)
                _disposable.Dispose();
            else
                RetryFetchComment();
        }

        private void RetryFetchComment()
        {
            _disposable?.Dispose();
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10))
                                    .Subscribe(async w => await FetchComment());
        }

        private async Task FetchComment()
        {
            StatusInfo.Instance.Text = "Fetching program comments (20 comments).";
            var comments = await _abemaApiHost.Comments(_abemaState.CurrentSlot.Id);
            if (comments.CommentList == null)
            {
                StatusInfo.Instance.Text = "Fetched program comment (0).";
                return;
            }
            foreach (var comment in comments?.CommentList.OrderBy(w => w.CreatedAtMs))
            {
                if (Comments.Any(w => w.Id == comment.Id) || comment.Message.Trim() == "")
                    continue;
                if (Comments.Count >= 200)
                    for (var i = 199; i < Comments.Count; i++)
                        Comments.RemoveAt(i);
                Comments.Insert(0, comment);
            }
            StatusInfo.Instance.Text = "Fetched program comments.";
        }
    }
}
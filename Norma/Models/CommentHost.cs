using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Norma.Gamma.Models;
using Norma.Helpers;
using Norma.Properties;

using Prism.Mvvm;

#pragma warning disable 0414

namespace Norma.Models
{
    internal class CommentHost : BindableBase, IDisposable
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly AbemaState _abemaState;
        private readonly CompositeDisposable _compositeDisposable;
        private readonly Configuration _configuration;
        private IDisposable _disposable; // for Comment synchronizer.

        public ObservableCollection<Comment> Comments { get; set; }

        public CommentHost(AbemaApiHost abemaApiHost, AbemaState abemaState, Configuration configuration)
        {
            Comments = new ObservableCollection<Comment>();
            _compositeDisposable = new CompositeDisposable();

            _abemaApiHost = abemaApiHost;
            _abemaState = abemaState;
            _configuration = configuration;
            _compositeDisposable.Add(_abemaState.Subscribe(nameof(_abemaState.CurrentSlot), w => ReloadComments()));
            // _compositeDisposable.Add(_abemaState.Subscribe(nameof(_abemaState.IsBroadcastCm), w => StopFetchComment()));
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

            var val = _configuration.Root.Operation.ReceptionIntervalOfComments;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val))
                                    .Subscribe(async w => await FetchComment());
        }

        /*
        private void StopFetchComment()
        {
            if (_abemaState.IsBroadcastCm)
                _disposable.Dispose();
            else
                RetryFetchComment();
        }
        */

        /*
        private void RetryFetchComment()
        {
            _disposable?.Dispose();

            var val = _configuration.Root.Operation.ReceptionIntervalOfComments;
            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(val))
                                    .Subscribe(async w => await FetchComment());
        }
        */

        private async Task FetchComment()
        {
            StatusInfo.Instance.Text = Resources.FetchingComments;
            var holdingComments = _configuration.Root.Operation.NumberOfHoldingComments;
            var comments = await _abemaApiHost.Comments(_abemaState.CurrentSlot.Id);
            if (comments?.CommentList == null)
            {
                if (comments != null)
                    StatusInfo.Instance.Text = Resources.FetchedComment0;
                return;
            }
            foreach (var comment in comments.CommentList.OrderBy(w => w.CreatedAtMs))
            {
                if (Comments.Any(w => w.Id == comment.Id) || comment.Message.Trim() == "" || IsMuteTarget(comment))
                    continue;
                if (Comments.Count >= holdingComments)
                    for (var i = holdingComments - 1; i < Comments.Count; i++)
                        Comments.RemoveAt((int) i);
                Comments.Insert(0, comment);
            }
            StatusInfo.Instance.Text = Resources.FetchedComments;
        }

        private bool IsMuteTarget(Comment comment)
        {
            return _configuration.Root.Operation.MuteKeywords.Any(w =>
            {
                if (!w.IsRegex)
                    return comment.Message.Contains(w.Keyword);
                var regex = new Regex(w.Keyword);
                return regex.IsMatch(comment.Message);
            });
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Norma.Eta.Extensions;
using Norma.Eta.Models;
using Norma.Eta.Properties;
using Norma.Eta.Services;
using Norma.Gamma.Models;

using Prism.Mvvm;

using Reactive.Bindings.Extensions;

#pragma warning disable 0414

namespace Norma.Models
{
    internal class CommentHost : BindableBase, IDisposable
    {
        private readonly AbemaApiClient _abemaApiHost;
        private readonly AbemaState _abemaState;
        private readonly CompositeDisposable _compositeDisposable;
        private readonly Configuration _configuration;
        private readonly StatusService _statusService;
        private IDisposable _disposable; // for Comment synchronizer.

        public ObservableCollection<Comment> Comments { get; }

        public CommentHost(AbemaApiClient abemaApiHost, AbemaState abemaState, Configuration configuration,
                           StatusService statusService)
        {
            Comments = new ObservableCollection<Comment>();
            _compositeDisposable = new CompositeDisposable();

            _abemaApiHost = abemaApiHost;
            _abemaState = abemaState;
            _configuration = configuration;
            _statusService = statusService;
            _compositeDisposable.Add(_abemaState.ObserveProperty(w => w.CurrentSlot).Subscribe(w => ReloadComments()));
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
            _disposable = Observable.Timer(TimeSpanExt.OneSecond, TimeSpan.FromSeconds(val))
                                    .Subscribe(async w => await FetchComment());
        }

        private async Task FetchComment()
        {
            _statusService.UpdateStatus(Resources.FetchingComments);
            var holdingComments = _configuration.Root.Operation.NumberOfHoldingComments;
            var comments = await _abemaApiHost.Comments(_abemaState.CurrentSlot.SlotId);
            if (comments?.CommentList == null)
            {
                if (comments != null)
                    _statusService.UpdateStatus(Resources.FetchedComment0);
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
            _statusService.UpdateStatus(Resources.FetchedComments);
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
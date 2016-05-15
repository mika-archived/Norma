using System;
using System.Reactive.Linq;

using Norma.Extensions;
using Norma.Models;
using Norma.ViewModels.Internal;

using Prism.Commands;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentInputViewModel : ViewModel
    {
        private readonly AbemaState _abemaState;
        public ReactiveProperty<string> Comment { get; }
        public ReadOnlyReactiveProperty<bool> IsEnableCommentInput { get; }

        public AbemaCommentInputViewModel(AbemaState abemaState)
        {
            _abemaState = abemaState;
            Comment = new ReactiveProperty<string>("").AddTo(this);
            Comment.Subscribe(w => SendCommentCommand.RaiseCanExecuteChanged()).AddTo(this);
            IsEnableCommentInput = _abemaState.ObserveProperty(w => w.IsBroadcastCm)
                                              .Select(w => !w)
                                              .ToReadOnlyReactiveProperty().AddTo(this);
        }

        #region SendCommentCommand

        private DelegateCommand _sendCommentCommand;

        public DelegateCommand SendCommentCommand
            => _sendCommentCommand ?? (_sendCommentCommand = new DelegateCommand(Send, CanSend));

        private async void Send()
        {
            await AbemaApiHost.Instance.Comment(_abemaState.CurrentSlot.Id, Comment.Value);
            Comment.Value = "";
        }

        private bool CanSend() => !string.IsNullOrWhiteSpace(Comment.Value);

        #endregion
    }
}
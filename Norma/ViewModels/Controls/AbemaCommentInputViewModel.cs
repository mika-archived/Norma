using System;

using Norma.Extensions;
using Norma.ViewModels.Internal;

using Prism.Commands;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentInputViewModel : ViewModel
    {
        public ReactiveProperty<string> Comment { get; }

        public AbemaCommentInputViewModel()
        {
            Comment = new ReactiveProperty<string>("").AddTo(this);
            Comment.Subscribe(w => SendCommentCommand.RaiseCanExecuteChanged()).AddTo(this);
        }

        // (ﾟДﾟ)ﾊｧ?
        public void OnProgramChanged(string title) => IsEnableCommentInput = title != "(CM)";

        #region IsEnableCommentInput

        private bool _isEnableCommentInput;

        public bool IsEnableCommentInput
        {
            get { return _isEnableCommentInput; }
            set { SetProperty(ref _isEnableCommentInput, value); }
        }

        #endregion

        #region SendCommentCommand

        private DelegateCommand _sendCommentCommand;

        public DelegateCommand SendCommentCommand
            => _sendCommentCommand ?? (_sendCommentCommand = new DelegateCommand(Send, CanSend));

        private void Send()
        {

        }

        private bool CanSend() => !string.IsNullOrWhiteSpace(Comment.Value);

        #endregion
    }
}
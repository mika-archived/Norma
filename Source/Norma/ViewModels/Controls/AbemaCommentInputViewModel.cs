using System;
using System.Windows.Input;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Prism.Commands;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentInputViewModel : ViewModel
    {
        private readonly AbemaApiHost _abemaApiHost;
        private readonly AbemaState _abemaState;
        private readonly Configuration _configuration;
        public ReactiveProperty<string> Comment { get; }
        // public ReadOnlyReactiveProperty<bool> IsEnableCommentInput { get; }

        public AbemaCommentInputViewModel(AbemaApiHost abemaApiHost, AbemaState abemaState, Configuration configuration)
        {
            _abemaApiHost = abemaApiHost;
            _abemaState = abemaState;
            _configuration = configuration;
            Comment = new ReactiveProperty<string>("").AddTo(this);
            Comment.Subscribe(w => SendCommentCommand.RaiseCanExecuteChanged()).AddTo(this);
            /*
            IsEnableCommentInput = _abemaState.ObserveProperty(w => w.IsBroadcastCm)
                                              .Select(w => !w)
                                              .ToReadOnlyReactiveProperty().AddTo(this);
            */
        }

        #region SendCommentCommand

        private DelegateCommand _sendCommentCommand;

        public DelegateCommand SendCommentCommand
            => _sendCommentCommand ?? (_sendCommentCommand = new DelegateCommand(Send, CanSend));

        private async void Send()
        {
            await _abemaApiHost.Comment(_abemaState.CurrentSlot.Id, Comment.Value);
            Comment.Value = "";
        }

        private bool CanSend() => !string.IsNullOrWhiteSpace(Comment.Value);

        #endregion

        #region OnKeyInputCommand

        private ICommand _onKeyInputCommand;

        public ICommand OnKeyInputCommand =>
            _onKeyInputCommand ?? (_onKeyInputCommand = new DelegateCommand<KeyEventArgs>(OnKeyInput));

        private void OnKeyInput(KeyEventArgs e)
        {
            if (!_configuration.Root.Operation.PostKeyType.IsMatchShortcut(e))
                return;
            if (CanSend())
                Send();
        }

        #endregion
    }
}
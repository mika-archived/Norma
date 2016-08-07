using System.Windows;
using System.Windows.Input;

using Microsoft.Practices.ServiceLocation;

using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Gamma.Models;
using Norma.Models;

using Prism.Commands;

namespace Norma.ViewModels
{
    internal class CommentViewModel : ViewModel
    {
        private readonly Comment _comment;
        private readonly Configuration _configuration;

        public string Message => _comment.Message.Trim();

        public string CreatedAt { get; }

        public CommentViewModel(Comment comment)
        {
            _comment = comment;
            var abemaState = ServiceLocator.Current.GetInstance<AbemaState>();
            _configuration = ServiceLocator.Current.GetInstance<Configuration>();
            CreatedAt = _configuration.Root.Operation.IsAbsoluteTime
                ? _comment.CreatedAtMs.ToString("HH:mm:ss")
                : (_comment.CreatedAtMs - abemaState.CurrentSlot.StartAt).ToString(@"hh\:mm\:ss");
        }

        #region AddToNgComment

        private ICommand _addToNgCommentCommand;

        // ReSharper disable once UnusedMember.Global
        public ICommand AddToNgCommentCommand
            => _addToNgCommentCommand ?? (_addToNgCommentCommand = new DelegateCommand(AddToNgComment));

        private void AddToNgComment()
            => _configuration.Root.Operation.MuteKeywords.Add(new MuteKeyword
            {
                IsRegex = true,
                Keyword = $"^{Message}$"
            });

        #endregion

        #region CopyToClipboardCommand

        private ICommand _copyToClipboardCommand;

        // ReSharper disable once UnusedMember.Global
        public ICommand CopyToClipboardCommand
            => _copyToClipboardCommand ?? (_copyToClipboardCommand = new DelegateCommand(CopyToClipboard));

        private void CopyToClipboard()
            => Clipboard.SetText(Message);

        #endregion
    }
}
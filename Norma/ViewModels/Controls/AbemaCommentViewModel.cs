using System.Collections.ObjectModel;

using Norma.Gamma.Models;
using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        private readonly CommentHost _commentHost;

        public ObservableCollection<Comment> Comments { get; private set; }

        public AbemaCommentViewModel(AbemaHostViewModel hostViewModel)
        {
            _commentHost = new CommentHost();
            Comments = new ObservableCollection<Comment>();

            hostViewModel.Subscribe(nameof(hostViewModel.Address), w =>
            {
                if (!hostViewModel.Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                _commentHost.OnChannelChanged(AbemaChannelExt.FromUrlString(hostViewModel.Address));
            });
        }

        // むー
        public void OnProgramChanged(string title) => _commentHost.OnProgramChanged(title);
    }
}
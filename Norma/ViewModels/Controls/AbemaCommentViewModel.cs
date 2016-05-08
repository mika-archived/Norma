using Norma.Extensions;
using Norma.Gamma.Models;
using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        private readonly CommentHost _commentHost;

        public ReadOnlyReactiveCollection<Comment> Comments { get; }

        public AbemaCommentViewModel(AbemaHostViewModel hostViewModel)
        {
            _commentHost = new CommentHost().AddTo(this);
            Comments = _commentHost.Comments.ToReadOnlyReactiveCollection();

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
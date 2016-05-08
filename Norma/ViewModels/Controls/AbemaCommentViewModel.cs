using System.Reactive.Linq;

using Norma.Extensions;
using Norma.Helpers;
using Norma.Models;
using Norma.ViewModels.Internal;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        private readonly CommentHost _commentHost;

        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }
        public ReactiveProperty<bool> IsEnableCommentArea { get; }

        public AbemaCommentViewModel(AbemaHostViewModel hostViewModel)
        {
            _commentHost = new CommentHost().AddTo(this);
            Comments = _commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w));
            IsEnableCommentArea = _commentHost.ObserveProperty(w => w.IsCm)
                                              .Select(w => !w).ToReactiveProperty();

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
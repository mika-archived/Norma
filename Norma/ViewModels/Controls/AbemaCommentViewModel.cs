using System.Reactive.Linq;
using System.Threading.Tasks;

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
        public ReadOnlyReactiveProperty<bool> IsEnableCommentArea { get; }

        public AbemaCommentViewModel(AbemaHostViewModel hostViewModel)
        {
            _commentHost = new CommentHost().AddTo(this);
            Comments = _commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
            IsEnableCommentArea = _commentHost.ObserveProperty(w => w.IsCm)
                                              .Select(w => !w).ToReadOnlyReactiveProperty().AddTo(this);

            hostViewModel.Subscribe(nameof(hostViewModel.Address), w =>
            {
                if (!hostViewModel.Address.StartsWith("https://abema.tv/now-on-air/"))
                    return;
                _commentHost.OnChannelChanged(AbemaChannelExt.FromUrlString(hostViewModel.Address));
            });
        }

        // むー
        public async Task OnProgramChanged(string title) => await _commentHost.OnProgramChanged(title);
    }
}
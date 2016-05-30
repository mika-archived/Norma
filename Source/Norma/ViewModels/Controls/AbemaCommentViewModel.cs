using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }
        // public ReadOnlyReactiveProperty<bool> IsEnableCommentArea { get; }

        public AbemaCommentViewModel(AbemaApiHost abemaApiHost, AbemaState abemaState, Configuration configuration)
        {
            var commentHost = new CommentHost(abemaApiHost, abemaState, configuration).AddTo(this);
            Comments = commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
            /*
            IsEnableCommentArea = abemaState.ObserveProperty(w => w.IsBroadcastCm)
                                            .Select(w => !w)
                                            .ToReadOnlyReactiveProperty().AddTo(this);
            */
        }
    }
}
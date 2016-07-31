using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Models;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }

        public AbemaCommentViewModel(AbemaApiHost abemaApiHost, AbemaState abemaState, Configuration c)
        {
            var commentHost = new CommentHost(abemaApiHost, abemaState, c).AddTo(this);
            Comments = commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
        }
    }
}
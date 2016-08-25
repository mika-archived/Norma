using Norma.Eta.Models;
using Norma.Eta.Mvvm;
using Norma.Eta.Services;
using Norma.Models;

using Reactive.Bindings;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }

        public AbemaCommentViewModel(AbemaApiClient abemaApiHost, AbemaState abemaState, Configuration configuration,
                                     StatusService statusService)
        {
            var commentHost = new CommentHost(abemaApiHost, abemaState, configuration, statusService).AddTo(this);
            Comments = commentHost.Comments.ToReadOnlyReactiveCollection(w => new CommentViewModel(w)).AddTo(this);
        }
    }
}
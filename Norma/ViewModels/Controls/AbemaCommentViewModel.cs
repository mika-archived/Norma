using System.Collections.ObjectModel;

using Norma.Gamma.Models;
using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        private readonly CommentHost _commentHost;

        public ObservableCollection<Comment> Comments { get; private set; }

        public AbemaCommentViewModel()
        {
            _commentHost = new CommentHost();
            Comments = new ObservableCollection<Comment>();
        }
    }
}
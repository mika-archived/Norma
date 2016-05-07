using System.Collections.ObjectModel;

using Norma.Gamma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    internal class AbemaCommentViewModel : ViewModel
    {
        public ObservableCollection<Comment> Comments { get; private set; }

        public AbemaCommentViewModel()
        {
            Comments = new ObservableCollection<Comment>();
        }
    }
}
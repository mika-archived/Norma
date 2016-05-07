using System.Collections.ObjectModel;

using Norma.Gamma.Models;

namespace Norma.Models
{
    internal class CommentHost
    {
        public ObservableCollection<Comment> Comments { get; set; }

        public CommentHost()
        {
            Comments = new ObservableCollection<Comment>();
        }
    }
}
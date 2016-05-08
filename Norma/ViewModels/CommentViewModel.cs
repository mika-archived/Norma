using Norma.Gamma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels
{
    internal class CommentViewModel : ViewModel
    {
        private readonly Comment _comment;

        public string Message => _comment.Message;

        public string CreatedAt => _comment.CreatedAtMs.ToString("HH:mm:ss");

        public CommentViewModel(Comment comment)
        {
            _comment = comment;
        }
    }
}
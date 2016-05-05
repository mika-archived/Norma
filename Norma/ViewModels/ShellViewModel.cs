using Norma.ViewModels.Internal;

namespace Norma.ViewModels
{
    internal class ShellViewModel : ViewModel
    {
        public string Title { get; set; }

        public ShellViewModel()
        {
            Title = "Norma - AbemaTV";
        }
    }
}
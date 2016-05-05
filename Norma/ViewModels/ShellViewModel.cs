using Norma.ViewModels.Internal;

namespace Norma.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ShellViewModel : ViewModel
    {
        public string Title { get; set; }

        public ShellViewModel()
        {
            Title = "Norma - AbemaTV";
        }
    }
}
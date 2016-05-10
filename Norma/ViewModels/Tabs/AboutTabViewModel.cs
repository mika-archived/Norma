using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Tabs
{
    internal class AboutTabViewModel : ViewModel
    {
        public string Name => ProductInfo.Name;

        public string Version => $"Version {ProductInfo.Version}";

        public string Copyright => ProductInfo.Copyright;
    }
}
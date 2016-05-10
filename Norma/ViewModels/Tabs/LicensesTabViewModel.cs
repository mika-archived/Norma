using System.Collections.Generic;
using System.Linq;

using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Tabs
{
    internal class LicensesTabViewModel : ViewModel
    {
        public List<LibraryViewModel> Libraries => ProductInfo.Libraries.Select(w => new LibraryViewModel(w)).ToList();
    }
}
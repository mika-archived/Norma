using System.Collections.Generic;
using System.Linq;

using Norma.Eta.Mvvm;
using Norma.Models;

namespace Norma.ViewModels.Tabs
{
    internal class LicensesTabViewModel : ViewModel
    {
        public List<LibraryViewModel> Libraries
            => ProductInfo.Libraries.Value.Select(w => new LibraryViewModel(w)).ToList();
    }
}
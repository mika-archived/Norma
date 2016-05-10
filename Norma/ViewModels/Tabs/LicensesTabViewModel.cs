using System.Collections.Generic;

using Norma.Models;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Tabs
{
    internal class LicensesTabViewModel : ViewModel
    {
        public List<Library> Libraries => ProductInfo.Libraries;
    }
}
using System;

using Norma.Eta;
using Norma.Eta.Mvvm;
using Norma.Models;

namespace Norma.ViewModels.Tabs
{
    internal class AboutTabViewModel : ViewModel
    {
        public string Name => ProductInfo.Name;

        public string Version => $"Version {ProductInfo.Version} {ProductInfo.ReleaseType.ToVersionString()}".Trim();

        public string Copyright => ProductInfo.Copyright;

        public string IsSupportNewToast => $"IsSupportNewToast = {NormaConstants.IsSupportedNewToast}";

        public string IsSupportToast => $"IsSupportToast = {NormaConstants.IsSupportedToast}";

        public string OsVersion => $"OsVersion = {Environment.OSVersion}";
    }
}
using System.Collections.Generic;
using System.Globalization;

using Norma.Eta.Properties;

using Prism.Mvvm;

namespace Norma.Eta.Models.Configurations
{
    public class InternalConfig : BindableBase
    {
        public bool IsTopMost { get; set; }

        public int Volume { get; set; }

        public List<string> FavoriteChannels { get; set; }

        public InternalConfig()
        {
            IsTopMost = false;
            Volume = 100;
            Lang = "ja"; // 手動で変えれば、 en にもなる。
            FavoriteChannels = new List<string>();
        }

        #region Lang

        private string _lang;

        public string Lang
        {
            get { return _lang; }
            set
            {
                if (SetProperty(ref _lang, value))
                    Resources.Culture = CultureInfo.GetCultureInfo(value);
            }
        }

        #endregion
    }
}
using Norma.Eta.Properties;

using Prism.Mvvm;

namespace Norma.Models
{
    public class StatusInfo : BindableBase
    {
        private StatusInfo()
        {
            Text = Resources.Ready;
        }

        #region Instance

        private static StatusInfo _instance;
        public static StatusInfo Instance => _instance ?? (_instance = new StatusInfo());

        #endregion

        #region Text

        private string _text;

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion
    }
}
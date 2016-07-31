using Norma.Eta.Properties;

using Prism.Mvvm;

namespace Norma.Eta.Services
{
    public class StatusService : BindableBase
    {
        public StatusService()
        {
            Status = Resources.Ready;
        }

        public void UpdateStatus(string str)
        {
            Status = str;
        }

        #region Status

        private string _status;

        public string Status
        {
            get { return _status; }
            private set { SetProperty(ref _status, value); }
        }

        #endregion
    }
}
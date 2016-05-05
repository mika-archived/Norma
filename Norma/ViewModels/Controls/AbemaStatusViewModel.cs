using Norma.ViewModels.Internal;

namespace Norma.ViewModels.Controls
{
    internal class AbemaStatusViewModel : ViewModel
    {
        public AbemaStatusViewModel()
        {
            Text = "Ready";
        }

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
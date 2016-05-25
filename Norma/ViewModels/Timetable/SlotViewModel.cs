using System.Windows.Input;

using Norma.Models.Timetables;
using Norma.ViewModels.Internal;

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Norma.ViewModels.Timetable
{
    internal class SlotViewModel : ViewModel
    {
        private readonly Slot _model;

        public string Title => _model.Model.Title;
        public string StartAt => _model.StartAt.ToString("HH:mm");
        public string EndAt => _model.EndAt.ToString("HH:mm");
        public string Description => _model.Model.TableHighlight;
        public int Height { get; private set; }
        public int Top { get; private set; }
        public InteractionRequest<INotification> ProgramDetailsRequest { get; }

        public SlotViewModel(Slot program)
        {
            _model = program;
            ProgramDetailsRequest = new InteractionRequest<INotification>();
            var span = _model.EndAt - _model.StartAt;
            Height = span.Hours * 60 * 3 + span.Minutes * 3;
            Top = _model.StartAt.Hour * 60 * 3 + _model.StartAt.Minute * 3;
        }

        #region OnMouseDownCommand

        private ICommand _onMouseDownCommand;

        public ICommand OnMouseDownCommand =>
            _onMouseDownCommand ?? (_onMouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnMouseDown));

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;
            ProgramDetailsRequest.Raise(new Notification {Content = "Blank", Title = "Blank"});
        }

        #endregion
    }
}
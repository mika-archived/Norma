using Norma.Models.Timetables;
using Norma.ViewModels.Internal;

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

        public SlotViewModel(Slot program)
        {
            _model = program;
            var span = _model.EndAt - _model.StartAt;
            Height = span.Hours * 60 * 3 + span.Minutes * 3;
            Top = _model.StartAt.Hour * 60 * 3 + _model.StartAt.Minute * 3;
        }
    }
}
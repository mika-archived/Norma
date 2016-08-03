using Norma.Eta.Mvvm;
using Norma.Iota.Models;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Norma.Iota.ViewModels.Controls
{
    internal class EpisodeCellViewModel : ViewModel
    {
        public WrapSlot Model { get; }

        public string Title => Model.Model.Title;
        public string StartAt => Model.StartAt.ToString("HH:mm");
        public string Description => Model.Model.Highlight;
        public int Height { get; private set; }
        public int Top { get; private set; }

        public EpisodeCellViewModel(WrapSlot program)
        {
            Model = program;
            var span = Model.EndAt - Model.StartAt;
            Height = span.Hours * 60 * 3 + span.Minutes * 3;
            Top = Model.StartAt.Hour * 60 * 3 + Model.StartAt.Minute * 3;
        }
    }
}
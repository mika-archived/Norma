using Norma.Eta.Mvvm;
using Norma.Iota.Models;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Norma.Iota.ViewModels.Controls
{
    internal class EpisodeCellViewModel : ViewModel
    {
        public WrapSlot Model { get; }

        public string Title => Model.Title;
        public string StartAt => Model.FixedStartAt.ToString("HH:mm");
        public string Highlight => Model.Highlight;
        public int Height { get; private set; }
        public int Top { get; private set; }

        public EpisodeCellViewModel(WrapSlot program)
        {
            Model = program;
            var span = Model.FixedEndAt - Model.FixedStartAt;
            Height = span.Hours * 60 * 3 + span.Minutes * 3;
            Top = Model.FixedStartAt.Hour * 60 * 3 + Model.FixedStartAt.Minute * 3;
        }
    }
}
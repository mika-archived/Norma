using System.Collections.Generic;

using Norma.Models.Timetables;
using Norma.ViewModels.Internal;

namespace Norma.ViewModels.WindowContents
{
    internal class SlotDetailsContentViewModel : ViewModel
    {
        private readonly Slot _model;
        public string Title => _model.Model.Title;
        public string Date => _model.StartAt.ToString("MM/DD");

        public string Time
            => $"{_model.Model.StartAt.ToString("MM/dd HH:mm")} ～ {_model.Model.EndAt.ToString("MM/dd HH:mm")}";

        public string Description => _model.DetailHighlight;
        public List<string> Cast => _model.Cast;
        public List<string> Staff => _model.Staff;

        public string Thumbnail
            => $"https://hayabusa.io/abema/programs/{_model.Model.DisplayProgramId}/thumb001.w200.h112.jpg";

        public SlotDetailsContentViewModel(Slot slot)
        {
            _model = slot;
        }
    }
}
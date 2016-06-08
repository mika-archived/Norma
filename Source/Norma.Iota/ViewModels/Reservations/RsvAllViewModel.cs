using Norma.Eta.Models.Reservations;

namespace Norma.Iota.ViewModels.Reservations
{
    internal class RsvAllViewModel : ReservationViewModel
    {
        // RsvProgram, RsvTime
        public string StartAt => ((RsvAll) Reserve).StartDate?.ToString("g");

        // RsvProgram
        public string ProgramId => ((RsvAll) Reserve).ProgramId;

        // RsvTime, RsvKeyword
        public string ExpiredAt => ((RsvAll) Reserve).Range.Finish.ToString("g");

        // RsvTime
        public string DayOfWeek => ((RsvAll) Reserve).DayOfWeek.ToLocaleString();

        // RsvKeyword
        public string Keyword => ((RsvAll) Reserve).Keyword;

        // RsvKeyword
        public bool IsRegexMode => ((RsvAll) Reserve).IsRegexMode;

        // Manage
        public string Type => ((RsvAll) Reserve).Type;

        public RsvAllViewModel(RsvAll reserve) : base(reserve)
        {

        }
    }
}
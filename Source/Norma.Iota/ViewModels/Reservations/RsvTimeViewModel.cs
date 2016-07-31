namespace Norma.Iota.ViewModels.Reservations
{
    internal class RsvTimeViewModel : ReservationViewModel
    {
        public string EndAt => ((RsvTime) Reserve).Range.Finish.ToString("d");

        public string StartTime => ((RsvTime) Reserve).StartTime.ToString("d");

        public string RepetitionType => ((RsvTime) Reserve).DayOfWeek.ToLocaleString();

        public RsvTimeViewModel(RsvTime reserve) : base(reserve)
        {

        }
    }
}
using Norma.Eta.Models.Reservations;

namespace Norma.Iota.ViewModels.Reservations
{
    internal class RsvProgramViewModel : ReservationViewModel
    {
        public string StartAt => ((RsvProgram) Reserve).StartDate.ToString("g");

        public string ProgramId => ((RsvProgram) Reserve).ProgramId;

        public RsvProgramViewModel(RsvProgram reserve) : base(reserve)
        {

        }
    }
}
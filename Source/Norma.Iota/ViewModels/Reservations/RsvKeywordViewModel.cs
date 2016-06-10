using Norma.Eta.Models.Reservations;

namespace Norma.Iota.ViewModels.Reservations
{
    internal class RsvKeywordViewModel : ReservationViewModel
    {
        public string EndAt => ((RsvKeyword) Reserve).Range.Finish.ToString("d");

        public string Keyword => ((RsvKeyword) Reserve).Keyword;

        public bool IsRegexMode => ((RsvKeyword) Reserve).IsRegexMode;

        public RsvKeywordViewModel(RsvKeyword reserve) : base(reserve)
        {

        }
    }
}
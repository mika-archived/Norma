namespace Norma.Eta.Models.Reservations
{
    public class RsvSeries : Reserve
    {
        /// <summary>
        ///     シリーズ ID
        /// </summary>
        public string SeriesId { get; set; }

        public RsvSeries()
        {
            Type = nameof(RsvSeries);
        }
    }
}
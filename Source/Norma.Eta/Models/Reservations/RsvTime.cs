using System;

namespace Norma.Eta.Models.Reservations
{
    /// <summary>
    ///     詳細予約 -> 時間予約
    /// </summary>
    public class RsvTime : Reserve
    {
        /// <summary>
        ///     有効期間
        /// </summary>
        public DateRange Range { get; set; }

        /// <summary>
        ///     予約時間
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     繰り返し
        /// </summary>
        public RepetitionType DayOfWeek { get; set; }
    }
}
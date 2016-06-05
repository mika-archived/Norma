using System;

namespace Norma.Eta.Models.Reservations
{
    /// <summary>
    ///     視聴予約(簡易)
    /// </summary>
    public class RsvProgram : Reserve
    {
        /// <summary>
        ///     予約開始時間
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        ///     対象番組ID
        /// </summary>
        public string ProgramId { get; set; }

        public RsvProgram()
        {
            Type = nameof(RsvProgram);
        }
    }
}
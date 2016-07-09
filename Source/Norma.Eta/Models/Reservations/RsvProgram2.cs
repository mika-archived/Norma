namespace Norma.Eta.Models.Reservations
{
    /// <summary>
    ///     視聴予約(公式)
    /// </summary>
    public class RsvProgram2 : Reserve
    {
        /// <summary>
        ///     対象番組ID
        /// </summary>
        public string ProgramId { get; set; }

        public RsvProgram2()
        {
            Type = nameof(RsvProgram2);
        }
    }
}
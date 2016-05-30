namespace Norma.Eta.Models.Reservations
{
    /// <summary>
    ///     詳細予約 -> キーワード予約
    /// </summary>
    public class RsvKeyword : Reserve
    {
        /// <summary>
        ///     有効期間
        /// </summary>
        public DateRange Range { get; set; }

        /// <summary>
        ///     キーワード
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        ///     正規表現モード
        /// </summary>
        public bool IsRegexMode { get; set; }

        public RsvKeyword()
        {
            IsRegexMode = false;
        }
    }
}
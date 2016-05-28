using Norma.Eta.Properties;

namespace Norma.Eta.Models.Reservations
{
    public enum RepetitionType
    {
        /// <summary>
        ///     月曜日
        /// </summary>
        Monday,

        /// <summary>
        ///     火曜日
        /// </summary>
        Tuesday,

        /// <summary>
        ///     水曜日
        /// </summary>
        Wednesday,

        /// <summary>
        ///     木曜日
        /// </summary>
        Thursday,

        /// <summary>
        ///     金曜日
        /// </summary>
        Friday,

        /// <summary>
        ///     土曜日
        /// </summary>
        Saturday,

        /// <summary>
        ///     日曜日
        /// </summary>
        Sunday,

        /// <summary>
        ///     毎日
        /// </summary>
        Everyday,

        /// <summary>
        ///     月曜日～金曜日
        /// </summary>
        MonToFri,

        /// <summary>
        ///     月曜日～土曜日
        /// </summary>
        MonToSat
    }

    public static class RepetitionTypeExt
    {
        public static string ToLocaleString(this RepetitionType obj)
        {
            return (string) typeof(Resources).GetProperty(obj.ToString()).GetValue(null);
        }
    }
}
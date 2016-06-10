using System;

using Norma.Eta.Properties;

namespace Norma.Eta.Models.Reservations
{
    public enum RepetitionType
    {
        /// <summary>
        ///     繰り返しなし
        /// </summary>
        None,

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

        public static bool IsMatch(this RepetitionType obj, DateTime date)
        {
            switch (obj)
            {
                case RepetitionType.None:
                case RepetitionType.Everyday:
                    return true;

                case RepetitionType.Monday:
                    return date.DayOfWeek == DayOfWeek.Monday;

                case RepetitionType.Tuesday:
                    return date.DayOfWeek == DayOfWeek.Tuesday;

                case RepetitionType.Wednesday:
                    return date.DayOfWeek == DayOfWeek.Wednesday;

                case RepetitionType.Thursday:
                    return date.DayOfWeek == DayOfWeek.Thursday;

                case RepetitionType.Friday:
                    return date.DayOfWeek == DayOfWeek.Friday;

                case RepetitionType.Saturday:
                    return date.DayOfWeek == DayOfWeek.Saturday;

                case RepetitionType.Sunday:
                    return date.DayOfWeek == DayOfWeek.Sunday;

                case RepetitionType.MonToFri:
                    return 1 <= (int) date.DayOfWeek && (int) date.DayOfWeek <= 5;

                case RepetitionType.MonToSat:
                    return 1 <= (int) date.DayOfWeek && (int) date.DayOfWeek <= 6;
            }
            return false;
        }
    }
}
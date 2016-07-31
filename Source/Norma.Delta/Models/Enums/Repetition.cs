using System;

namespace Norma.Delta.Models.Enums
{
    public enum Repetition
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
        public static bool IsMatch(this Repetition obj, DateTime date)
        {
            switch (obj)
            {
                case Repetition.None:
                case Repetition.Everyday:
                    return true;

                case Repetition.Monday:
                    return date.DayOfWeek == DayOfWeek.Monday;

                case Repetition.Tuesday:
                    return date.DayOfWeek == DayOfWeek.Tuesday;

                case Repetition.Wednesday:
                    return date.DayOfWeek == DayOfWeek.Wednesday;

                case Repetition.Thursday:
                    return date.DayOfWeek == DayOfWeek.Thursday;

                case Repetition.Friday:
                    return date.DayOfWeek == DayOfWeek.Friday;

                case Repetition.Saturday:
                    return date.DayOfWeek == DayOfWeek.Saturday;

                case Repetition.Sunday:
                    return date.DayOfWeek == DayOfWeek.Sunday;

                case Repetition.MonToFri:
                    return 1 <= (int) date.DayOfWeek && (int) date.DayOfWeek <= 5;

                case Repetition.MonToSat:
                    return 1 <= (int) date.DayOfWeek && (int) date.DayOfWeek <= 6;

                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
    }
}
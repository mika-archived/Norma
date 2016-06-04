using System;

namespace Norma.Eta.Models
{
    public static class DateTimeHelper
    {
        public static bool EqualsWithDates(DateTime obj1, DateTime obj2)
        {
            return obj1.Year == obj2.Year && obj1.Month == obj2.Month && obj1.Day == obj2.Day;
        }

        public static bool EqualsWithHours(DateTime obj1, DateTime obj2)
        {
            return obj1.Hour == obj2.Hour && obj1.Minute == obj2.Minute;
        }

        public static bool IsRangeOf(DateTime startAt, DateTime endAt, DateTime target)
        {
            return startAt <= target && target <= endAt;
        }
    }
}
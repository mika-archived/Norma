using System;

namespace Norma.Delta.Models
{
    public class DateRange
    {
        public static readonly DateRange Unspecified = new DateRange
        {
            StartAt = DateTime.MinValue,
            EndAt = DateTime.MaxValue
        };

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }
    }
}
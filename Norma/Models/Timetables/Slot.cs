using System;

using BaseSlot = Norma.Gamma.Models.Slot;

namespace Norma.Models.Timetables
{
    internal class Slot
    {
        public BaseSlot Model { get; }

        // 調節
        public DateTime StartAt { get; }

        public DateTime EndAt { get; }

        public Slot(BaseSlot slot, DateTime date)
        {
            Model = slot;
            StartAt = Model.StartAt < date ? new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) : Model.StartAt;
            EndAt = Model.EndAt >= date.AddDays(1)
                ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
                : Model.EndAt;
        }
    }
}
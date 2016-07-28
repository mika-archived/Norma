using System;
using System.Collections.Generic;
using System.Linq;

using Norma.Gamma.Models;

namespace Norma.Iota.Models
{
    internal class WrapSlot
    {
        public Slot Model { get; }

        // 調節
        public DateTime StartAt { get; }

        public DateTime EndAt { get; }

        public bool CanRsv { get; }

        public string DetailHighlight { get; }

        public List<string> Cast { get; }
        public List<string> Staff { get; }

        public WrapSlot(Slot slot, DateTime date)
        {
            Model = slot;
            StartAt = Model.StartAt < date ? new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) : Model.StartAt;
            EndAt = Model.EndAt >= date.AddDays(1)
                ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)
                : Model.EndAt;

            if (Model.Programs.Length > 0)
            {
                DetailHighlight = Model.DetailHighlight;
                Cast = Model.Programs[0].Credit.Cast?.ToList();
                Staff = Model.Programs[0].Credit.Crews?.ToList();
            }
            else
            {
                Cast = new List<string>();
                Staff = new List<string>();
                if (!string.IsNullOrWhiteSpace(Model.DetailHighlight))
                    DetailHighlight = Model.DetailHighlight;
                else if (!string.IsNullOrWhiteSpace(Model.Highlight))
                    DetailHighlight = Model.Highlight;
                else
                    DetailHighlight = Model.TableHighlight;
            }
            CanRsv = StartAt > DateTime.Now.AddMinutes(5);
        }
    }
}
using System;
using System.Collections.Generic;

namespace Norma.Delta.Models
{
    public class Slot
    {
        public string SlotId { get; set; }

        public string Title { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public string Highlight { get; set; }

        public string HighlightDetail { get; set; }

        public string Description { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        public virtual Channel Channel { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
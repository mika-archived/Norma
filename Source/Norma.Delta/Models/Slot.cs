using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Norma.Delta.Models
{
    public class Slot : IEqualityComparer<Slot>
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

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public Slot()
        {
            Episodes = new List<Episode>();
        }

        #region Implementation of IEqualityComparer<in Slot>

        public bool Equals(Slot x, Slot y) => x.SlotId == y.SlotId;

        public int GetHashCode(Slot obj) => obj.SlotId.GetHashCode();

        #endregion
    }
}
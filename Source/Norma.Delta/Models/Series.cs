using System.Collections.Generic;

namespace Norma.Delta.Models
{
    public class Series
    {
        public string SeriesId { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
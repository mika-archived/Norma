using System.Collections.Generic;

namespace Norma.Delta.Models
{
    /// <summary>
    ///     個別の
    /// </summary>
    public class Episode
    {
        public string EpisodeId { get; set; }

        public int Sequence { get; set; }

        public virtual ICollection<Cast> Casts { get; set; }

        public virtual ICollection<Crew> Crews { get; set; }

        public virtual ICollection<Copyright> Copyrights { get; set; }

        public virtual ICollection<Thumbnail> Thumbnails { get; set; }

        public virtual Series Series { get; set; }

        public virtual ICollection<Slot> Slots { get; set; }
    }
}
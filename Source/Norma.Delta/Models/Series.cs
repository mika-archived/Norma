using System.Collections.Generic;

namespace Norma.Delta.Models
{
    public class Series
    {
        public string Id { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }

        public Series()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Episodes = new List<Episode>();
        }
    }
}
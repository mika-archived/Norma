using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class Cast
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }

        public Cast()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Episodes = new List<Episode>();
        }
    }
}
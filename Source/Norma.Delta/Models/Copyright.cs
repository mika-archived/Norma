using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Norma.Delta.Models
{
    public class Copyright
    {
        public int CopyrightId { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
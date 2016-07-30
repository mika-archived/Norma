using System.ComponentModel.DataAnnotations;

namespace Norma.Eta.Models
{
    public class MigrationHistory
    {
        [Key]
        public string MigrationId { get; set; }
    }
}
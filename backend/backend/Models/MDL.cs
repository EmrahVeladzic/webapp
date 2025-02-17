using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("MDL",Schema ="Models")]
    public class MDL:BaseEntity
    {
        [NotMapped]
        public List<MSH>? Meshes { get; set; }
    }
}

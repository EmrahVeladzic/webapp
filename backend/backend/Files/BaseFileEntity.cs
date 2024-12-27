using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Files
{
    public class BaseFileEntity
    {
        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        [Column("EntityHash")]
        public string? Hash { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }
    }
}

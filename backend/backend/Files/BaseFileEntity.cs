using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Files
{
    public class BaseFileEntity
    {
        [Key]
        [Column("EntityID")]     
        public string? Hash { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }
    }
}

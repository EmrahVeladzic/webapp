using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("GLB", Schema = "Files")]
    public class GLB
    {

        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        [Column("EntityHash")]
        public string? Hash { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

        [NotMapped]
        public string? Metadata { get; set; }


        [NotMapped]
        public byte[]? BLOB { get; set; }



        GLB()
        {

        }

    }
}

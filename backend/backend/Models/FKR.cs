using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("FKR",Schema ="Models")]
    public class FKR
    {
        [Key]
        [Column("EntityID")]
        public int Id { get; set; }

        [Column("EntityOrder")]
        public int Order {  get; set; }

        [Column("RigRoot")]
        public byte Root { get; set; }

    }
}

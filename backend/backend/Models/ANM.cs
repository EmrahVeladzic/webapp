using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("ANM",Schema ="Models")]
    public class ANM:BaseEntity
    {
        [Column("FKRID")]
        [ForeignKey(nameof(FKR))]
        public int FKR_ID { get; set; }

    }
}

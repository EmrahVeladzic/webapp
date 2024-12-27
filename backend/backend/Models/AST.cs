using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("AST",Schema ="Models")]
    public class AST:BaseEntity
    {
        [ForeignKey(nameof(MDL))]
        [Column("MDLID")]
        public int MDL_ID { get; set; }

        [ForeignKey(nameof(FKR))]
        [Column("FKRID")]
        public int FKR_ID { get; set; }

    }
}

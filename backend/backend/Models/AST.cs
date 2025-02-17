using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("AST",Schema ="Models")]
    public class AST:BaseEntity
    {
       
        [Column("MDLID")]
        public int MDL_ID { get; set; }

       
        [Column("FKRID")]
        public int FKR_ID { get; set; }

        
        [ForeignKey(nameof(MDL_ID))]
        public virtual MDL? MDL {  get; set; }

        [ForeignKey(nameof(FKR_ID))]
        public virtual FKR? FKR { get; set; }

        [Column("ShiftBits")]
        public byte ShiftBits { get; set; }

    }
}

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

        [Column("PrecisionBits")]
        public byte PrecisionBits { get; set; }


        public AST():base()
        {
           this.MDL = new MDL();
           this.FKR = new FKR();
        }

        public override void Serialize()
        {

            this.MDL?.Serialize();
            this.FKR?.Serialize();
        }
    }
}

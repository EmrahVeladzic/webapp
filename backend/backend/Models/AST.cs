using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("AST",Schema ="Models")]
    public class AST:BaseEntity
    {
        [JsonIgnore]
        [Column("MDLID")]
        public int? MDL_ID { get; set; }

        [JsonIgnore]
        [Column("FKRID")]
        public int? FKR_ID { get; set; }

        
        [ForeignKey(nameof(MDL_ID))]
        public virtual MDL? MDL {  get; set; }

        [ForeignKey(nameof(FKR_ID))]
        public virtual FKR? FKR { get; set; }

        [Column("PrecisionBits")]
        public byte PrecisionBits { get; set; }


        public AST():base()
        {
            this.MDL = null;
            this.FKR = null;

            this.MDL_ID = null;
            this.FKR_ID = null;
        }

        public override void Serialize()
        {

            this.MDL?.Serialize();
            this.FKR?.Serialize();
        }
    }
}

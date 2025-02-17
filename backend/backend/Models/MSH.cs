using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("MSH",Schema ="Models")]
    public class MSH:BaseEntity
    {
       
        [Column("MDLID")]
        public int MDL_ID { get; set; }

       
        [Column("VTID")]
        public int VT_ID { get; set; }

       
        [Column("INDID")]
        public int IND_ID { get; set; }

       
        [Column("UVID")]
        public int UV_ID { get; set; }

      
        [Column("NRMID")]
        public int NRM_ID { get; set; }

     
        [Column("BNID")]
        public int BN_ID { get; set; }

        [ForeignKey(nameof(VT_ID))]
        public virtual VT? VT { get; set; }

        [ForeignKey(nameof(IND_ID))]
        public virtual IND? IND { get; set; }

        [ForeignKey(nameof(NRM_ID))]
        public virtual NRM? NRM { get; set; }

        [ForeignKey(nameof(UV_ID))]
        public virtual UV? UV { get; set; }


    }
}

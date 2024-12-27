using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("MSH",Schema ="Models")]
    public class MSH:BaseEntity
    {
        [ForeignKey(nameof(MDL))]
        [Column("MDLID")]
        public int MDL_ID { get; set; }

        [ForeignKey(nameof(VT))]
        [Column("VTID")]
        public int VT_ID { get; set; }

        [ForeignKey(nameof(IND))]
        [Column("INDID")]
        public int IND_ID { get; set; }

        [ForeignKey(nameof(UV))]
        [Column("UVID")]
        public int UV_ID { get; set; }

        [ForeignKey(nameof(NRM))]
        [Column("NRMID")]
        public int NRM_ID { get; set; }

        [ForeignKey(nameof(BN))]
        [Column("BNID")]
        public int BN_ID { get; set; }


    }
}

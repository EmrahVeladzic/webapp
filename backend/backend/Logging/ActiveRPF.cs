using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Logging
{
    [Table("ActiveRPF", Schema = "Active")]
    public class ActiveRPF: BaseLogEntity
    {
        [Column("CLUTSize")]
        public byte CLUT {  get; set; }

        [Column("RAlpha")]
        public byte? Red {  get; set; }

        [Column("GAlpha")]
        public byte? Green { get; set; }

        [Column("BAlpha")]
        public byte? Blue { get; set; }

        [Column("AlphaPresent")]
        public bool AlphaPresent { get; set; }

        [Column("CompressionMode")]
        public bool Method {  get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Logging
{
    [Table("ActiveAST", Schema = "Active")]
    public class ActiveAST :BaseLogEntity
    {
        [Column("PrecisionBits")]
        public byte PrecisionBits { get; set; }

        [Column("FPS")]
        public byte? FPS { get; set; }

        [Column("TWidth")]
        public byte? Tex_Width { get; set; }

        [Column("THeight")]
        public byte? Tex_Height { get; set; }



    }
}

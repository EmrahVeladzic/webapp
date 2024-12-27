using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [Table("RPF", Schema ="Models")]
    public class RPF:BaseEntity
    {
        
        //The top-level image format. The foreign keys are converted to element offsets.
         

        //Size of lookup table (+1, as 0 is not a valid amount)
        [Column("CLUT")]
        public byte CLUT {  get; set; }


        [ForeignKey(nameof(PLT))]
        [Column("PLTID")]
        public int PLT_ID { get; set; }

        [NotMapped]
        public virtual PLT? PLT { get; set; }

        [ForeignKey(nameof(PGA))]
        [Column("PGAID")]
        public int PGA_ID { get; set; }

        [NotMapped]
        public virtual PGA? PGA { get; set; }


        //Width (+1, as values will range 2-256)
        [Column("Width")]
        public byte Width { get; set; }

        //Height (+1, as values will range 2-256). The product of width and height, relative to CLUT size will give the correct number of bytes to load
        [Column("Height")]
        public byte Height { get; set; }

      
    }
}

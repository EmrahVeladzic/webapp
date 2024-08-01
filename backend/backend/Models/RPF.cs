using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("RPF", Schema ="Models")]
    public class RPF
    {
        /*
         The top-level image format. The foreign keys are converted to element offsets.
         */

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Order")]
        public int Order { get; set; }

        //Size of lookup table (+1, as 0 is not a valid amount)
        [Column("CLUT")]
        public byte CLUT {  get; set; }

        [Column("PLT")]
        public int PLT_Id { get; set; }

        [NotMapped]
        public virtual PLT? PLT { get; set; }

        [Column("PGA")]
        public int PGA_Id { get; set; }

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

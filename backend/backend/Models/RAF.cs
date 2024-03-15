using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("RAF")]
    public class RAF
    {

        /*
            A set of overrides for the RPF. The foreign keys will become element offsets. The frame values are preserved as-is.
            Element size is 9 bytes in order => 1Frame + 4PLT_Offset (in elements) + 4PGA_Offset (in elements)

        */

        [Key]
        [Column("Id")]
        public UInt16 Id { get; set; }

        [Column("Order")]
        public UInt16 Order { get; set; }

        //Frame values go from 0 to 50/60 depending on the region Hz. Are relative (At trigger time, considered to be on frame 0)
        [Column("Frames")]
        public List<byte>? Frames { get; set; }



        [Column("PLTs")]
        public List<UInt16>? PLT_Ids { get; set; }

        [NotMapped]
        public virtual List<PLT>? PLTs { get; set; }
       
        [Column("PGAs")]
        public List<UInt16>? PGA_Ids { get; set; }

        [NotMapped]
        public virtual List<PGA>? PGAs { get; set; }

        public RAF()
        {
            PLT_Ids = new List<UInt16>();
            PLTs= new List<PLT>();
            PGA_Ids = new List<UInt16>();
            PGAs = new List<PGA>();

        }


    }
}

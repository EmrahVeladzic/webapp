using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("PLT")]
    public class PLT
    {
        //A Colour lookup table (CLUT). Element size is 2 bytes.

        [Key]
        [Column("Id")]
        public UInt16 Id { get; set; }

        [Column("Order")]
        public UInt16 Order { get; set; }

        [Column("Data")]
        public List<UInt16>? Serialized { get; set; }

        [NotMapped]
        public List<Pixel15>? Data { get; set; }

        public PLT()
        {
            Serialized = new List<ushort>();
            Data = new List<Pixel15>();
        }

    }

  
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("PLT", Schema ="Models")]
    public class PLT
    {
        //A Colour lookup table (CLUT). Element size is 2 bytes.

        [Key]
        [Column("EntityId")]
        public int Id { get; set; }

        [Column("EntityOrder")]
        public int Order { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

        [NotMapped]
        public List<byte>? ToSerialize { get; set; }

        [NotMapped]
        public List<Pixel15>? Data { get; set; }


        public PLT()
        {
            Data = new List<Pixel15>();
            ToSerialize = new List<byte>();
        }

    }

  
}

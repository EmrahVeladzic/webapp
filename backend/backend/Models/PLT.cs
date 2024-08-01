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
        public List<byte>? SerializedData { get; set; }

        [NotMapped]
        public List<UInt16>? Data { get; set; }


        public PLT()
        {
            Data = new List<UInt16>();
            SerializedData = new List<byte>();
        }

    }

  
}

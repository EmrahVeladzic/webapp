using backend.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("PGA", Schema ="Models")]
    public class PGA
    {
        //Indexed image data. Points to slots in the CLUT. Element size is 1 byte by default but can represent multiple pixels. 

        [Key]
        [Column("EntityId")]
        public int Id { get; set; }

        [Column("EntityOrder")]
        public int Order { get; set; }

        [Column("EntityData")]
        public List<byte>? Data { get; set; }


        public PGA()
        {
            Data = new List<byte>();
        }

    }


  
}

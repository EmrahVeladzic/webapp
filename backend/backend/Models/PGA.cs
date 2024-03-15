using backend.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [Table("PGA")]
    public class PGA
    {
        //Indexed image data. Points to slots in the CLUT. Element size is 1 byte by default but can represent multiple pixels. 

        [Key]
        [Column("Id")]
        public UInt16 Id { get; set; }

        [Column("Order")]
        public UInt16 Order { get; set; }

        [Column("Data")]
        public List<byte>? Data { get; set; }


        public PGA()
        {
            Data = new List<byte>();
        }

    }


  
}

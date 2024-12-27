using backend.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [Table("PGA", Schema ="Models")]
    public class PGA:BaseBufferEntity
    {
        //Indexed image data. Points to slots in the CLUT. Element size is 1 byte by default but can represent multiple pixels. 


        [NotMapped]
        public List<byte>? Data { get; set; }


        public PGA()
        {
            Data = new List<byte>();
        }

    }


  
}

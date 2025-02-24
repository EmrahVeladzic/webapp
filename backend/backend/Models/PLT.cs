using backend.Converters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [Table("PLT", Schema = "Models")]
    public class PLT : BaseBufferEntity
    {
        //A Colour lookup table (CLUT). Element size is 2 bytes.


        [NotMapped]
        public List<Pixel15>? Data { get; set; }


        public PLT() : base()
        {
            Data = new List<Pixel15>();
        }


        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (Pixel15 pxl in this.Data!)
            {
                pxl.Serialize(this.ToSerialize!);
            }


            this.Serialized = this.ToSerialize.ToArray();

        }

    }

  
}

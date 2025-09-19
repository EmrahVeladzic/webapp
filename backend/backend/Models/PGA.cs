using backend.Converters;
using backend.Database;
using backend.Files;
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

        public PGA():base()
        {
            
        }

        public void Serialize(IMG_DATA IMG)
        {
            this.ToSerialize = new List<byte>();

            byte value = 0;

            Pixel15 compare = new Pixel15();

            for (int i = 0; i < IMG!.Image!.Data!.Count; i++)
            {
                compare.Setup(IMG!.Image!.Data![i], !IMG!.Image!.Data![i].Equals(IMG!.Alpha!));

                if (IMG!.Image!.Data![i].Equals(IMG!.Alpha!))
                {
                    compare.Data = 0x0000;

                }


                value <<= IMG.Shift_Value;

                value |= IMG.Get_Index(compare);



                if (IMG!.Shift_Value == 0 || (IMG!.Shift_Value != 0 && ((i + 1) % (8 / IMG!.Shift_Value) == 0)))
                {
                    this.ToSerialize.Add(value);

                    value = 0;


                }

            }

            this.Serialized = this.ToSerialize.ToArray();
        }

        public  override void Deserialize()
        {
            this.ToSerialize=this.Serialized!.ToList();

        }

        public override void Clear()
        {
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }

    }


  
}

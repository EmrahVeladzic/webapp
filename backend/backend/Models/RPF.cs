using backend.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [Table("RPF", Schema ="Models")]
    public class RPF:BaseEntity,ITopLevelModel
    {
        
        //The top-level image format. The foreign keys are converted to element offsets.
         

        //Size of lookup table (+1, as 0 is not a valid amount)
        [Column("CLUT")]
        public byte CLUT {  get; set; }


        [Column("PLTID")]
        public int PLT_ID { get; set; }

      
        [ForeignKey(nameof(PLT_ID))]
        public virtual PLT? PLT { get; set; }

      
        [Column("PGAID")]
        public int PGA_ID { get; set; }

        
        [ForeignKey(nameof(PGA_ID))]
        public virtual PGA? PGA { get; set; }


        //Width (+1, as values will range 2-256)
        [Column("Width")]
        public byte Width { get; set; }

        //Height (+1, as values will range 2-256). The product of width and height, relative to CLUT size will give the correct number of bytes to load
        [Column("Height")]
        public byte Height { get; set; }

        public RPF():base()
        {
            PGA = new PGA();
            PLT = new PLT();
        }

        public void Serialize(IMG_DATA IMG)
        {
            this.PLT?.Serialize();
            this.PGA?.Serialize(IMG);            
        }

        public override void Deserialize()
        {
            this.PLT?.Deserialize();
            this.PGA?.Deserialize();
        }

        public override void Clear()
        {
            this.PGA?.Clear();
            this.PLT?.Clear();

            this.PLT = null;
            this.PGA = null;
        }

        public byte[] ToArrayBuffer()
        {
            List<byte> temp = new List<byte>();

            temp.Add(82);
            temp.Add(CLUT);
            temp.Add(Width);
            temp.Add(Height);

            if (PLT != null)
            {
                foreach(Pixel15 p in PLT.Data!)
                {
                    p.Serialize(temp);
                }
            }

            if (PGA != null)
            {
                temp.AddRange(PGA.Serialized!);
            }


            byte[] data = temp.ToArray();
            temp.Clear();
            return data;
        }
    }
}

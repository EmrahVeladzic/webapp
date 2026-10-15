using backend.Converters;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [Table("RPF", Schema = "Models")]
    public class RPF : BaseEntity, ITopLevelModel
    {

        //The top-level image format. The foreign keys are converted to element offsets.


        //Size of lookup table (+1, as 0 is not a valid amount)
        [Column("CLUT")]
        public byte CLUT { get; set; }


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

        [Column("TexRow")]
        public bool TexRow { get; set; }

        [Column("TexX")]
        public Int16 TexX { get; set; }


        [Column("TexY")]
        public byte TexY { get; set; }

        [Column("CLUTRow")]
        public bool CLUTRow { get; set; }

        [Column("CLUTX")]
        public Int16 CLUTX { get; set; }


        [Column("CLUTY")]
        public byte CLUTY { get; set; }

        public RPF() : base()
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
            temp.Add(0);
            temp.Add(0);
            temp.Add(0);

            UInt32 divisor = (UInt32)((this.CLUT > 15) ? 2 : 4);

            UInt32 tex_line = (UInt32)(this.TexRow ? 256 : 0) + (UInt32)this.TexY;
            UInt32 tex_span = ((UInt32)this.Width + 1) / divisor;

            UInt32 clut_line = (UInt32)(this.CLUTRow ? 256 : 0) + (UInt32)this.CLUTY;
            UInt32 clut_span = (UInt32)this.CLUT + 1;

            UInt32 dest_tex = ((tex_line << 16) & 0xFFFF0000) | ((UInt32)(UInt16)this.TexX & 0x0000FFFF);
            UInt32 size_tex = (((((UInt32)this.Height) + 1) << 16) & 0xFFFF0000) | (tex_span & 0x0000FFFF);

            UInt32 dest_clut = ((clut_line << 16) & 0xFFFF0000) | ((UInt32)(UInt16)this.CLUTX & 0x0000FFFF);
            UInt32 size_clut = ((UInt32)1 << 16) | (clut_span & 0x0000FFFF);

            UInt16 texpage = (UInt16)(((((UInt32)(UInt16)this.TexX) >> 6) & 0x000F)
                                    | ((UInt32)(this.TexRow ? 1 : 0) << 4)
                                    | ((UInt32)((this.CLUT > 15) ? 1u : 0u) << 7));

            UInt16 clut_word = (UInt16)(((((UInt32)(UInt16)this.CLUTX) >> 4) & 0x003F)
                                    | ((clut_line & 0x01FF) << 6));

            PrimitiveSerialization.SerializePrimitive(dest_tex, temp);
            PrimitiveSerialization.SerializePrimitive(size_tex, temp);
            PrimitiveSerialization.SerializePrimitive(dest_clut, temp);
            PrimitiveSerialization.SerializePrimitive(size_clut, temp);
            PrimitiveSerialization.SerializePrimitive(texpage, temp);
            PrimitiveSerialization.SerializePrimitive(clut_word, temp);

            if (PLT != null)
            {
                foreach (Pixel15 p in PLT.Data!)
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
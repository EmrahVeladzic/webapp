using backend.Utils;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential,Pack =4)]
    [Keyless]
    public class Pixel15
    {
        
        //A "15-bit" RGBA format. Little endian.  
        public UInt16 Data { get; set; }

        public void Setup(Pixel24 input , bool alpha)
        {
            byte R = (byte)((int)input.Red / 8);
            byte G = (byte)((int)input.Green / 8);
            byte B = (byte)((int)input.Blue / 8);



            if (alpha)
            {
                this.Data = (UInt16)(1 | (B<<11) | (G<<6) | (R<<1));
            }

            else
            {
                this.Data = (UInt16)(0 | (B << 11) | (G << 6) | (R << 1));
            }
        }
            
            
        
        public Pixel15(Pixel24 input, bool alpha)
        {
            Setup(input, alpha);
        }

        public Pixel15()
        {
            this.Data = 0;
        }

        public Pixel15(UInt16 input)
        {
            this.Data = input;
        }

        public int Red()
        {
            return (int)((this.Data>>1)&0x001F);
        }

        public int Green()
        {
            return (int)((this.Data >> 6) & 0x001F);
        }

        public int Blue()
        {
            return (int)((this.Data >> 11) & 0x001F);
        }

        public int Alpha()
        {
            return (int)(this.Data & 0x0001);
        }

        public void Swap(Pixel15 input)
        {
            this.Data = input.Data;
        }

        public override string ToString()
        {
            return $"{this.Red()} {this.Green()} {this.Blue()} {this.Alpha()}";
        }

        public bool Equals(Pixel15 other)
        {
            if (other == null) { return false; }
            else
            {
               return this.Data == other.Data;

            }
        }

        public void Serialize(List<byte> output)
        {
           PrimitiveSerialization.SerializePrimitive(Data, output);
        }

    }
}

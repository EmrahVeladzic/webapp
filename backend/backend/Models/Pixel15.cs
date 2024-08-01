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

        public void Setup(Pixel24 input)
        {
            byte R = (byte)((int)input.Red / 8);
            byte G = (byte)((int)input.Green / 8);
            byte B = (byte)((int)input.Blue / 8);

            //The user is the only one with the ability to define which texel will be transparent. 
            //By default the conversion 24=>15 will result in a opaque texel.
            


            // The format is LE and goes as : A BBBBB GGGGG RRRRR

            this.Data = (UInt16)((1<< 15) | (B << 14) | (G << 9) | (R << 4));
            
            
        }
        public Pixel15(Pixel24 input)
        {
            Setup(input);
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
            return (int)(this.Data & 0x1F);
        }

        public int Green()
        {
            return (int)(this.Data & 0x3E0);
        }

        public int Blue()
        {
            return (int)(this.Data & 0x7C00);
        }

        public int Alpha()
        {
            return (int)(this.Data & 0x8000);
        }

        public void Swap(Pixel15 input)
        {
            this.Data = input.Data;
        }

        //Checks to see if colour is already user-defined. Overwrites alpha if necessary;
        public void Assert_User_Defined(UInt16 usr_def)
        {
            if ((int)(this.Data & 0x7FFF) == (int)(usr_def & 0x7FFF))
            {
                this.Data=usr_def;
            }
        }

    }
}

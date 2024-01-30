using System.Runtime.InteropServices;

namespace backend.Models
{
    [StructLayout(LayoutKind.Sequential,Pack =4)]
    public class Pixel15
    {        
        public Int16 Data { get; set; }

        public void Setup(Pixel24 input)
        {
            byte R = (byte)((int)input.Red / 8);
            byte G = (byte)((int)input.Green / 8);
            byte B = (byte)((int)input.Blue / 8);

            if (input.Alpha)
            {
                this.Data = (Int16)((0<<15)|(B << 11) | (G << 6) | (R << 1));
            }
            else
            {
                this.Data = (Int16)((1<< 15) | (B << 11) | (G << 6) | (R << 1));
            }
            
        }
        public Pixel15(Pixel24 input)
        {
            Setup(input);
        }
    }
}

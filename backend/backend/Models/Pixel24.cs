using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace backend.Models
{
    [Keyless]
    public class Pixel24
    {
        //A standard 24-bit pixel structure. 

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Pixel24(byte r, byte g, byte b)
        {
            this.Red = r;
            this.Green = g;
            this.Blue = b;
        }

        public Pixel24()
        {

        }

        public bool Equals(Pixel24 other)
        {
            if (other != null)
            {

                return ((other.Red == this.Red) && (other.Green == this.Green) && (other.Blue == this.Blue));

            }

            else { return false; }
        }


        public override string ToString()
        {
            return $"{Red} {Green} {Blue}";
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Keyless]
    public class Pixel24
    {
        //A standard 24-bit pixel structure. 

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Pixel24()
        {
            
        }

    }
}

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

        //User defined. Images do not contain Alpha channels on their own.
        public bool Alpha { get; set; }
        public Pixel24()
        {
            
        }

    }
}

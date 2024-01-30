namespace backend.Models
{
    public class Pixel24
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        //User defined - Image needs to be 24-bit.
        public bool Alpha { get; set; }        

    }
}

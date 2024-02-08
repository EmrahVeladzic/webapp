namespace backend.Models
{
    public struct Pixel24
    {
        //A standard 24-bit pixel structure. 

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        //User defined - Image needs to be 24-bit.
        public bool Alpha { get; set; }        

    }
}

namespace backend.Requests
{
    public class ImageDTO
    {
        //Complete BMP data (incl. Header). Obtain texture dimensions from here.
        
        public  string? ImageData { get; set; }

        //SHA-256 Encoded.
        public string? ImageHash { get; set; }

        //Bits per pixel. Add 1 to this value.
        public byte CLUT_Size { get; set; }

        //If not NULL, use this colour as the designated alpha = 0. Format = 24-bit RGB.
        public List<byte>? Alpha { get; set; }
        
        //Determines the compression method. 0 = Popularity, 1 = Proximity.
        public bool Mode {  get; set; }

        //Determines the size of the protected buffer to be used with the Proximity method. 
        public byte ProtectedBufferSize { get; set; }

     

        
    }    

}

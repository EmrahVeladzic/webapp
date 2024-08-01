using backend.Models;

namespace backend.Requests
{
    public class ImageJson
    {
        //Complete BMP data (incl. Header). Obtain texture dimensions from here.
        public  string? ImageData { get; set; }

        //Bits per pixel.
        public UInt16 CLUT_Size { get; set; }

        //If not NULL, use this colour as the designated alpha = 0. Format = 24-bit RGB.
        public List<UInt16>? Alpha { get; set; }
        
        //Determines the compression method. 0 = Popularity, 1 = Proximity.
        public bool Mode {  get; set; }

        //Determines the size of the protected buffer to be used with the Proximity method. 
        public UInt16 ProtectedBufferSize { get; set; }
    }    

}

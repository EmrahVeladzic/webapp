using System.ComponentModel.DataAnnotations.Schema;

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

        //Location of the texture in VRAM. This parameter can be changed by anyone at any time
        public byte TexturePage_X { get; set; }
        public byte TexturePage_Y { get; set; }
        public byte TextureOffset_X { get; set; }
        public byte TextureOffset_Y { get; set; }

    }    

}

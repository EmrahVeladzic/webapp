namespace backend.Requests
{
    public class TextureDTO
    {     
       
        //CLUT size per frame. +1, as 0 is not valid. 
        public byte Colours { get; set; }

        //Dimensions. +1, as 0 is not valid.
        public byte Width { get; set; }
        public byte Height { get; set; }
        
        //CLUT. 
        public List<UInt16>? CLUT { get; set; }

        //Pixel grid.
        public List<byte>? Pixels { get; set; }

        public int RPF_ID { get; set; }

        public bool CanDelete { get; set; }

        public bool TexRow { get; set; }
        public UInt16 Tex_X { get; set; }
        public byte Tex_Y { get; set; }

        public bool CLUTRow { get; set; }
        public UInt16 CLUT_X { get; set; }
        public byte CLUT_Y { get; set; }


    }
}

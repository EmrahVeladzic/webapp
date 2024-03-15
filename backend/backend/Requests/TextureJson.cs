namespace backend.Requests
{
    public class TextureJson
    {     
        //"R" - implies that the file has something to do with rasterized images
        public byte Magic { get; set; }

        //CLUT size per frame. +1, as 0 is not valid. 
        public byte Colours { get; set; }

        //Dimensions. +1, as 0 is not valid.
        public byte Width { get; set; }
        public byte Height { get; set; }

        //Multiplying the above gets bytes per frame.


        //CLUT. 
        public List<UInt16>? CLUT { get; set; }


        //Pixel grid.
        public List<byte>? Pixels { get; set; }


        //CLUT frames.
        public List<byte>? CFrames { get; set; }

        //Grid frames.
        public List<byte>? PFrames { get; set; }

    }
}

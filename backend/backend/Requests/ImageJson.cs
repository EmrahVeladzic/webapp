namespace backend.Requests
{
    public class ImageJson
    {
        //Complete BMP data (incl. Header)
        public  string? ImageData { get; set; }

        //Number of unique colours (image-wise).
        //Result may be lower - This is the maximum.
        //Needs to take in account the reserved colours. Validation done client-side.
        public  byte UniqueTexelCount { get; set; }
        
        //These colours will be fed into the CLUT preemptively. Converted into PXL15 client-side.
        //Has to be <=255.

        public  List<UInt16>? ReservedClut { get; set; }

        //Number of partitions to be made on the image. Helps preserve unique colours. 2^0 - 2^4.
        public  byte XPartition { get; set; }

        //Number of partitions to be made on the image. Helps preserve unique colours. 2^0 - 2^4.
        public byte YPartition { get; set; }

        //Number of unique colours that a chunk may contribute. Can be 0%-100% of the total texture colour count. 
        //Needs to be < UniqueTexelCount. If there are <Max Colours in the chunk, the next valid chunk inherits the differential.
        //Sorted from the top-left to the bottom right chunk.
        public List<byte>? MaxColoursPerChunk { get; set; }

        //Pecking order of the chunks. Helps prevent less visually important chunks from hogging the CLUT.
        //Like the above list, is sorted from the top-left to the bottom right chunk.
        public List<byte>? ChunkOrder { get; set; }
    }    

}

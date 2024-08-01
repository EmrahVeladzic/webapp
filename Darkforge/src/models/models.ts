export class ImageJson{
    // Complete BMP data (incl. Header)
    ImageData: string;
    // Number of unique colours (image-wise).
    // Result may be lower - This is the maximum.
    // Needs to take in account the reserved colours. Validation done client-side.
    UniqueTexelCount: number;
    // These colours will be fed into the CLUT preemptively. Converted into PXL15 client-side.
    // Has to be <=255.
    ReservedClut: number[];
    // Number of partitions to be made on the image. Helps preserve unique colours. 2^0 - 2^4.
    XPartition: number;
    // Number of partitions to be made on the image. Helps preserve unique colours. 2^0 - 2^4.
    YPartition: number;
    // Number of unique colours that a chunk may contribute. Can be 0%-100% of the total texture colour count.
    // Needs to be < UniqueTexelCount. If there are <Max Colours in the chunk, the next valid chunk inherits the differential.
    // Sorted from the top-left to the bottom right chunk.
    MaxColoursPerChunk: number[];
    // Pecking order of the chunks. Helps prevent less visually important chunks from hogging the CLUT.
    // Like the above list, is sorted from the top-left to the bottom right chunk.
    ChunkOrder: number[];

    
    constructor(data:string, utc : number, res: number[], X:number, Y:number, maxcpc:number[], ord:number[]) {
        
        this.ImageData=data;
        this.UniqueTexelCount=utc;
        this.ReservedClut=res;
        this.XPartition=X;
        this.YPartition=Y;
        this.MaxColoursPerChunk=maxcpc;
        this.ChunkOrder=ord;        
    }
}
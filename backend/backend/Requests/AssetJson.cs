using backend.Models;

namespace backend.Requests
{
    public class AssetJson
    {
        //Processed asset.
        public AST? Asset { get; set; }

        public int AST_ID { get; set; }

        //Root bone of FKR. Null if FKR is not present.
        public int? Root {  get; set; }
        
        public byte PrecisionBits { get; set; }

    }
}

using backend.Models;

namespace backend.Requests
{
    public class AssetDTO
    {
        //Processed asset.
        public AST? Asset { get; set; }

        public int AST_ID { get; set; }

        public bool CanDelete { get; set; }
    }
}

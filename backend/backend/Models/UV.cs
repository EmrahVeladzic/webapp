using backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("UV", Schema = "Models")]
    public class UV : BaseBufferEntity
    {
        [NotMapped]
        public List<FVector2> TextureCoordinates { get; set; }

        public UV()
        {
            this.TextureCoordinates = new List<FVector2>();
        }
    }
}

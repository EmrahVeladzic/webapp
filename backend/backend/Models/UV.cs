using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("UV", Schema = "Models")]
    public class UV : BaseBufferEntity
    {
        [NotMapped]
        public List<Vector2> TextureCoordinates { get; set; }

        public UV()
        {
            this.TextureCoordinates = new List<Vector2>();
        }
    }
}

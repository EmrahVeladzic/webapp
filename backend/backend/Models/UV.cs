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

        public UV():base()
        {
            this.TextureCoordinates = new List<FVector2>();
        }


        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (FVector2 t in this.TextureCoordinates)
            {
                t.Serialize(this.ToSerialize);
            }

            this.Serialized = this.ToSerialize.ToArray();
        }
    }
}

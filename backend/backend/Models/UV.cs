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
        public List<byte> TextureCoordinates { get; set; }     

        public UV():base()
        {
            this.TextureCoordinates = new List<byte>();
        }


        public override void Serialize(){
                   
            this.Serialized = this.TextureCoordinates.ToArray();
        }
    }
}

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
        public List<byte>? TextureCoordinates { get; set; }
        public UV():base()
        {
            this.TextureCoordinates = new List<byte>();
            this.ToSerialize = new List<byte>();
        }


        public override void Serialize(){

            this.ToSerialize = this.TextureCoordinates;
            this.Serialized = this.ToSerialize!.ToArray();
        }

        public override void Deserialize()
        {
           this.TextureCoordinates=this.Serialized!.ToList();           

           this.Serialized=null;
        }

        public override void Clear()
        {
            this.TextureCoordinates?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }
    }
}

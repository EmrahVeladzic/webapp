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

        public void Deserialize(byte Width, byte Height, byte clut_shift, byte x_off, byte y_off)
        {
            this.TextureCoordinates=this.Serialized!.ToList();           
            for(int i = 0; i < TextureCoordinates.Count; i += 2)
            {
                this.TextureCoordinates[i] /= (byte)(256 / (int)Width);
                this.TextureCoordinates[i+1] /= (byte)(256 / (int)Height);
            }

        }

        public override void Clear()
        {
            this.TextureCoordinates?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }
    }
}

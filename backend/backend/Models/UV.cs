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


        public UV() : base()
        {
            this.TextureCoordinates = new List<byte>();
            this.ToSerialize = new List<byte>();
        }


        public override void Serialize()
        {

            this.ToSerialize = this.TextureCoordinates;
            this.Serialized = this.ToSerialize!.ToArray();
        }

        public void Deserialize(byte Width, byte Height, byte x_off, byte y_off)
        {
            this.TextureCoordinates = this.Serialized!.ToList();
            for (int i = 0; i < TextureCoordinates.Count; i += 2)
            {
                this.TextureCoordinates[i] = (byte)Math.Min(x_off + ((this.TextureCoordinates[i] * (Width + 1)) + 127) / 255, 255);
                this.TextureCoordinates[i + 1] = (byte)Math.Min(y_off + ((this.TextureCoordinates[i + 1] * (Height + 1)) + 127) / 255, 255);
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
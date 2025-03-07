using backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("NRM", Schema = "Models")]
    public class NRM : BaseBufferEntity
    {
        [NotMapped]
        public List<Int32> Normals { get; set; }

        public NRM():base()
        {
            this.Normals = new List<Int32>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (Int32 n in this.Normals)
            {
                PrimitiveSerialization.SerializePrimitive(n,this.ToSerialize);
            }

            this.Serialized = this.ToSerialize.ToArray();
        }

        public override void Deserialize()
        {

            for (Int32 i = 0; i < this.Serialized!.Length; i += 4)
            {
                Int32 temp = BitConverter.ToInt32(this.Serialized!, i);
                this.Normals.Add(temp);
            }

            this.Serialized = null;
        }

        public override void Clear()
        {
            this.Normals?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }
    }
}

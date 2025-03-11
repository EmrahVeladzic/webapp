using backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("VT", Schema = "Models")]
    public class VT:BaseBufferEntity
    {
        [NotMapped]
        public List<Int32> Vertices { get; set; }

        public VT() : base()
        {
            this.Vertices = new List<Int32>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (Int32 v in this.Vertices)
            {
                PrimitiveSerialization.SerializePrimitive(v, this.ToSerialize);
            }

            this.Serialized=this.ToSerialize.ToArray();
        }

        public override void Deserialize()
        {
            for(Int32 i =0; i<this.Serialized!.Length; i += 4)
            {
                Int32 temp = BitConverter.ToInt32(this.Serialized!, i);
                this.Vertices.Add(temp);
            }


        }

        public override void Clear()
        {
            this.Vertices?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }
    }
}

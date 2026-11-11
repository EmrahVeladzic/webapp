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
        public List<Int16> Vertices { get; set; }

        public VT() : base()
        {
            this.Vertices = new List<Int16>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (Int16 v in this.Vertices)
            {
                PrimitiveSerialization.SerializePrimitive(v, this.ToSerialize);
            }

            this.Serialized=this.ToSerialize.ToArray();
        }

        public override void Deserialize()
        {
            for(Int32 i =0; i<this.Serialized!.Length; i += 2)
            {
                Int16 temp = BitConverter.ToInt16(this.Serialized!, i);
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

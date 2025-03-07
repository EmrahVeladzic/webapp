using backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("IND", Schema = "Models")]
    public class IND : BaseBufferEntity
    {
        [NotMapped]
        public List<UInt16> Indices { get; set; }

        public IND():base()
        {
            this.Indices = new List<UInt16>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (UInt16 i in this.Indices)
            {
                PrimitiveSerialization.SerializePrimitive(i,this.ToSerialize);
            }

            this.Serialized = this.ToSerialize.ToArray();
        }

        public override void Deserialize()
        {
            for (Int32 i = 0; i < this.Serialized!.Length; i += 2)
            {
                UInt16 temp = BitConverter.ToUInt16(this.Serialized!, i);
                this.Indices.Add(temp);
            }
            this.Serialized = null;
        }

        public override void Clear()
        {
            this.Indices?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized= null;
        }
    }
}

using backend.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("BN",Schema ="Models")]
    public class BN:BaseBufferEntity
    {
        [JsonIgnore]
        [Column("FKRID")]
        public int FKR_ID { get; set; }

        [Column("Parent")]
        public int? Parent_ID { get; set; }
     
        [NotMapped]
        public List<Int32> InitialTransform { get; set; }

        public BN():base()
        {
            
            InitialTransform = new List<Int32>();
            Parent_ID = null;

        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            for (int i = 0; i < 10; i++)
            {
                PrimitiveSerialization.SerializePrimitive(InitialTransform[i], this.ToSerialize);
            }

            this.Serialized=this.ToSerialize.ToArray();

        }


        public override void Deserialize()
        {
            for (int i = 0; i < this.Serialized!.Length; i += 4)
            {
                this.InitialTransform.Add(BitConverter.ToInt32(this.Serialized!, i));
            }

            this.Serialized=null;
        }

        public override void Clear()
        {
            this.InitialTransform!.Clear();
            this.ToSerialize!.Clear();
            this.Serialized = null;
        }
    }
}

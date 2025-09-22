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
        public List<Int32> InitialTranslation { get; set; }

        [NotMapped]
        public List<Int16> InitialRotation { get; set; }

        [NotMapped]
        public List<Int16> InitialScale{ get; set; }

        public BN():base()
        {
            
            InitialTranslation = new();
            InitialRotation = new();
            InitialScale = new();
            Parent_ID = null;

        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            for (int i = 0; i < 3; i++)
            {
                PrimitiveSerialization.SerializePrimitive(InitialTranslation[i], this.ToSerialize);
            }

            for (int i = 0; i < 4; i++)
            {
                PrimitiveSerialization.SerializePrimitive(InitialRotation[i], this.ToSerialize);
            }

            for (int i = 0; i < 3; i++)
            {
                PrimitiveSerialization.SerializePrimitive(InitialScale[i], this.ToSerialize);
            }


            this.Serialized=this.ToSerialize.ToArray();

        }


        public override void Deserialize()
        {
            for (int i = 0; i < 12; i += 4)
            {
                this.InitialTranslation.Add(BitConverter.ToInt32(this.Serialized!, i));
            }
            for (int i = 12; i < 20; i += 2)
            {
                this.InitialRotation.Add(BitConverter.ToInt16(this.Serialized!, i));
            }
            for (int i = 20; i < 26; i += 2)
            {
                this.InitialScale.Add(BitConverter.ToInt16(this.Serialized!, i));
            }

        }

        public override void Clear()
        {
            this.InitialTranslation.Clear();
            this.InitialRotation.Clear();
            this.InitialScale.Clear();
            this.ToSerialize!.Clear();
            this.Serialized = null;
        }
    }
}

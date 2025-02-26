using backend.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("TK",Schema ="Models")]
    public class TK:BaseBufferEntity
    {
        
        [Column("BNID")]
        public int BN_ID { get; set; }

        [JsonIgnore]
        [Column("ANMID")]
        public int ANM_ID { get; set; }

        [NotMapped]
        public List<Int32> Translations { get; set; }

        [NotMapped]
        public List<Int32> Rotations { get; set; }

        [NotMapped]
        public List<Int32> Scales { get; set; }

        [NotMapped]
        public List<byte> T_Frames { get; set; }

        [NotMapped]
        public List<byte> R_Frames { get; set; }

        [NotMapped]
        public List<byte> S_Frames { get; set; }


        public TK():base()
        {
            this.Translations = new List<Int32>();
            this.Rotations = new List<Int32>();
            this.Scales = new List<Int32>();

            this.T_Frames = new List<byte>();
            this.R_Frames = new List<byte>();
            this.S_Frames = new List<byte>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            for(int i = 0; i < this.T_Frames.Count; i++)
            {
                this.ToSerialize.Add(this.T_Frames[i]);

                PrimitiveSerialization.SerializePrimitive(this.Translations[(i*3)],this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Translations[(i*3)+1], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Translations[(i*3)+2], this.ToSerialize);


            }

            for (int i = 0; i < this.R_Frames.Count; i ++)
            {

                this.ToSerialize.Add(this.R_Frames[i]);

                PrimitiveSerialization.SerializePrimitive(this.Rotations[(i*4)], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Rotations[(i*4) + 1], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Rotations[(i*4) + 2], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Rotations[(i*4) + 3], this.ToSerialize);


            }

            for (int i = 0; i < this.S_Frames.Count; i ++)
            {
                this.ToSerialize.Add(this.S_Frames[i]);

                PrimitiveSerialization.SerializePrimitive(this.Scales[(i*3)], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Scales[(i*3) + 1], this.ToSerialize);
                PrimitiveSerialization.SerializePrimitive(this.Scales[(i*3) + 2], this.ToSerialize);


            }

            this.Serialized=this.ToSerialize.ToArray();
        }

    }
}

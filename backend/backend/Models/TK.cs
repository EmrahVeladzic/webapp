using backend.Utils;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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

        [JsonIgnore]
        [Column("T_Count")]
        public byte T_Count { get; set; }

        [JsonIgnore]
        [Column("R_Count")]
        public byte R_Count { get; set; }

        [JsonIgnore]
        [Column("S_Count")]
        public byte S_Count { get; set; }

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

        public override void Deserialize()
        {
          
            int offset = 0;

           
            for (int i = 0; i < (int)this.T_Count; i++)
            {
                this.T_Frames.Add(this.Serialized![offset]);
                offset += 1; 

                this.Translations.Add(BitConverter.ToInt32(this.Serialized!, offset));
                this.Translations.Add(BitConverter.ToInt32(this.Serialized!, offset + 4));
                this.Translations.Add(BitConverter.ToInt32(this.Serialized!, offset + 8));
                offset += 12; 
            }

           
            for (int i = 0; i < (int)this.R_Count; i++)
            {
                this.R_Frames.Add(this.Serialized![offset]);
                offset += 1; 

                this.Rotations.Add(BitConverter.ToInt32(this.Serialized!, offset));
                this.Rotations.Add(BitConverter.ToInt32(this.Serialized!, offset + 4));
                this.Rotations.Add(BitConverter.ToInt32(this.Serialized!, offset + 8));
                this.Rotations.Add(BitConverter.ToInt32(this.Serialized!, offset + 12));
                offset += 16; 
            }

           
            for (int i = 0; i < (int)this.S_Count; i++)
            {
                this.S_Frames.Add(this.Serialized![offset]);
                offset += 1; 

                this.Scales.Add(BitConverter.ToInt32(this.Serialized!, offset));
                this.Scales.Add(BitConverter.ToInt32(this.Serialized!, offset + 4));
                this.Scales.Add(BitConverter.ToInt32(this.Serialized!, offset + 8));
                offset += 12; 
            }
        }

        public override void Clear()
        {
            this.T_Frames?.Clear();
            this.Translations?.Clear();

            this.R_Frames?.Clear();
            this.Rotations?.Clear();

            this.S_Frames?.Clear();
            this.Scales?.Clear();

            this.ToSerialize?.Clear();
            this.Serialized = null;
        }

    }
}

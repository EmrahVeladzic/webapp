using backend.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("TK",Schema ="Models")]
    public class TK:BaseBufferEntity
    {
        
        [Column("BNID")]
        public int BN_ID { get; set; }

        [Column("ANMID")]
        public int ANM_ID { get; set; }

        [NotMapped]
        public List<FVector3> Translations { get; set; }

        [NotMapped]
        public List<FQuaternion> Rotations { get; set; }

        [NotMapped]
        public List<FVector3> Scales { get; set; }

        [NotMapped]
        public List<byte> T_Frames { get; set; }

        [NotMapped]
        public List<byte> R_Frames { get; set; }

        [NotMapped]
        public List<byte> S_Frames { get; set; }


        public TK():base()
        {
            this.Translations = new List<FVector3>();
            this.Rotations = new List<FQuaternion>();
            this.Scales = new List<FVector3>();

            this.T_Frames = new List<byte>();
            this.R_Frames = new List<byte>();
            this.S_Frames = new List<byte>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            for (int i = 0; i < this.Translations.Count; i++)
            {
                this.ToSerialize.Add(this.T_Frames[i]);

                this.Translations[i].Serialize(this.ToSerialize);

            }

            for (int i = 0; i < this.Rotations.Count; i++)
            {
                this.ToSerialize.Add(this.R_Frames[i]);

                this.Rotations[i].Serialize(this.ToSerialize);

            }

            for (int i = 0; i < this.Scales.Count; i++)
            {
                this.ToSerialize.Add(this.S_Frames[i]);

                this.Scales[i].Serialize(this.ToSerialize);

            }

            this.Serialized=this.ToSerialize.ToArray();
        }

    }
}

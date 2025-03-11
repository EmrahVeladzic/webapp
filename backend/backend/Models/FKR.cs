using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("FKR",Schema ="Models")]
    public class FKR:BaseEntity
    {
        [NotMapped]
        public List<BN> Bones { get; set; }

        [NotMapped]
        public List<ANM> Animations { get; set; }

        [Column("TargetFPS")]
        public byte? FPS { get; set; }

        [NotMapped]       
        public int? Root { get; set; }

        public FKR():base()
        {
            Bones = new List<BN>();
            Animations = new List<ANM>();
        }

        public override void Serialize()
        {
            foreach (BN b in Bones)
            {
                b.Serialize();
            }

            foreach (ANM a in Animations)
            {
                a.Serialize();
            }
        }

        public override void Deserialize()
        {
            foreach(BN b in Bones)
            {
                b.Deserialize();
            }
            foreach(ANM a in Animations)
            { 
                a.Deserialize();
            }

        }

        public override void Clear()
        {
            foreach(BN b in Bones)
            {
                b.Clear();
            }
            this.Bones.Clear();
            foreach(ANM a in Animations)
            {
                a.Clear();
            }
            this.Animations.Clear();

        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("MDL",Schema ="Models")]
    public class MDL:BaseEntity
    {
        [NotMapped]
        public List<MSH> Meshes { get; set; }

        [Column("TargetTextureWidth")]
        public byte Width { get; set; }

        [Column("TargetTextureHeight")]
        public byte Height { get; set; }

        [Column("TargetTexturePageX")]
        public byte PageX { get; set; }

        [Column("TargetTexturePageY")]
        public byte PageY { get; set; }

        [Column("TargetTextureOffsetX")]
        public byte OffsetX { get; set; }

        [Column("TargetTextureOffsetY")]
        public byte OffsetY { get; set; }

        [Column("TargetTextureClutXReduction")]
        public byte ClutXShift {  get; set; }


        public MDL() : base()
        {
            this.Meshes = new List<MSH>();
        }

        public override void Serialize()
        {
            foreach(MSH m in Meshes)
            {
                m.Serialize();
            }
        }

        public override void Deserialize()
        {
            foreach (MSH m in Meshes)
            {
                m.Deserialize(this.Width,this.Height,this.ClutXShift,this.OffsetX,this.OffsetY);
            }

        }

        public override void Clear()
        {
            foreach (MSH m in Meshes)
            {
                m.Clear();
            }

            Meshes.Clear();
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("MDL",Schema ="Models")]
    public class MDL:BaseEntity
    {
        [NotMapped]
        public List<MSH> Meshes { get; set; }

        [Column("TargetTextureWidth")]
        public byte? Width { get; set; }

        [Column("TargetTextureHeight")]
        public byte? Height { get; set; }


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
    }
}

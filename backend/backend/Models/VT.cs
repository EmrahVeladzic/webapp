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
        public List<FVector3> Vertices { get; set; }

        public VT() : base()
        {
            this.Vertices = new List<FVector3>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (FVector3 v in this.Vertices)
            {
                v.Serialize(this.ToSerialize);
            }

            this.Serialized=this.ToSerialize.ToArray();
        }
    }
}

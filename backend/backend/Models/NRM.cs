using backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("NRM", Schema = "Models")]
    public class NRM : BaseBufferEntity
    {
        [NotMapped]
        public List<FVector3> Normals { get; set; }

        public NRM():base()
        {
            this.Normals = new List<FVector3>();
        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            foreach (FVector3 n in this.Normals)
            {
                n.Serialize(this.ToSerialize);
            }

            this.Serialized = this.ToSerialize.ToArray();
        }
    }
}

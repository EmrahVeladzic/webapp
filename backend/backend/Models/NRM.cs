using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("NRM", Schema = "Models")]
    public class NRM : BaseBufferEntity
    {
        [NotMapped]
        public List<Vector3> Normals { get; set; }

        public NRM()
        {
            this.Normals = new List<Vector3>();
        }
    }
}

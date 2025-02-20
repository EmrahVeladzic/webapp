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

        public VT()
        {
            this.Vertices = new List<FVector3>();
        }
    }
}

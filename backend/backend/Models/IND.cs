using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace backend.Models
{
    [Table("IND", Schema = "Models")]
    public class IND : BaseBufferEntity
    {
        [NotMapped]
        public List<UInt16> Indices { get; set; }

        public IND()
        {
            this.Indices = new List<UInt16>();
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class BaseBufferEntity:BaseEntity
    {

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

        [NotMapped]
        public List<byte>? ToSerialize { get; set; }

        public BaseBufferEntity():base()
        {
            this.ToSerialize = new List<byte>();
            this.Serialized = this.ToSerialize.ToArray();
        }

      
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class BaseBufferEntity:BaseEntity
    {
        [JsonIgnore]
        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<byte>? ToSerialize { get; set; }

        public BaseBufferEntity():base()
        {
            this.ToSerialize = new List<byte>();
            this.Serialized = this.ToSerialize.ToArray();
        }

      
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("ANM",Schema ="Models")]
    public class ANM:BaseEntity
    {
        [JsonIgnore]
        [Column("FKRID")]
        public int FKR_ID { get; set; }

        [NotMapped]
        public List<TK> Tracks { get; set; }

        public ANM() : base()
        {
            Tracks = new List<TK>();

        }

        public override void Serialize()
        {
            foreach(TK t in Tracks)
            {
                t.Serialize();
            }
        }
    }
}

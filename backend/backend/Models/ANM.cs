using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("ANM",Schema ="Models")]
    public class ANM:BaseEntity
    {
        [Column("FKRID")]
        public int FKR_ID { get; set; }

        [NotMapped]
        public List<TK> Tracks { get; set; }

        public ANM()
        {
            Tracks = new List<TK>();
        }
    }
}

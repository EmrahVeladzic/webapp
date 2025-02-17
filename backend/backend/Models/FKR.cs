using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("FKR",Schema ="Models")]
    public class FKR:BaseEntity
    {
        [NotMapped]
        public List<BN> Bones { get; set; }

        [NotMapped]
        public List<ANM> Animations { get; set; }

        public FKR()
        {
            Bones = new List<BN>();
            Animations = new List<ANM>();
        }

    }
}

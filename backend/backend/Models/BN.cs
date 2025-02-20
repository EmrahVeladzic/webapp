using backend.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("BN",Schema ="Models")]
    public class BN:BaseBufferEntity
    {
        [Column("FKRID")]
        public int FKR_ID { get; set; }

        [Column("Parent")]
        public int? Parent_ID { get; set; }
     
        [NotMapped]
        public FTransform InitialTransform { get; set; }

        public BN()
        {
            InitialTransform = new FTransform();
        }


    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("BN",Schema ="Models")]
    public class BN:BaseBufferEntity
    {
        [Column("FKRID")]
        [ForeignKey(nameof(FKR))]
        public int FKR_ID { get; set; }

        [Column("Parent")]
        [ForeignKey(nameof(BN))]
        public int? Parent_ID { get; set; }


        [NotMapped]
        public Transform? InitialPosition { get; set; }


    }
}

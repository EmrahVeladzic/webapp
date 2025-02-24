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

        public BN():base()
        {
            
            InitialTransform = new FTransform(new Transform());
            Parent_ID = null;

        }

        public override void Serialize()
        {
            this.ToSerialize = new List<byte>();

            this.InitialTransform.Serialize(this.ToSerialize);

            this.Serialized=this.ToSerialize.ToArray();

        }

    }
}

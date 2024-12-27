using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    [Table("WL", Schema = "Models")]
    public class WL:BaseBufferEntity
    {
    
        [NotMapped]
        public List<ADPCMBlock>? Data { get; set; }

        [Column("WLCID")]
        [ForeignKey(nameof(WLC))]
        public int WLC_ID { get; set; }


        public WL()
        {
            this.Data = new List<ADPCMBlock>();
          
        }

    }
}

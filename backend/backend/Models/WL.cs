using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    [Table("WL", Schema = "Models")]
    public class WL:BaseBufferEntity
    {

       

        [Column("SampleRate")]
        public Int16 SerializedSampleRate { get; set; }

        [Column("BlockCountPerChannel")]
        public Int32 BlockCountPerChannel { get; set; }

        [NotMapped]
        public UInt16 SampleRate { get; set; }

        [Column("ChannelCount")]
        public byte ChannelCount { get; set; }

        [Column("ThresholdBits")]
        public byte ThresholdBits { get; set; }
       

        [NotMapped]
        public List<ADPCMBlock>? Data { get; set; }

        public WL()
        {
            this.Data = new List<ADPCMBlock>();
            this.ToSerialize= new List<byte> { };
        }

    }
}

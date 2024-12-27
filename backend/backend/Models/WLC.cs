using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class WLC:BaseEntity
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
               
        public WLC()
        {
         
        }


    }
}

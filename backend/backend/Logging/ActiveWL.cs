using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Logging
{
    [Table("ActiveWL", Schema = "Active")]
    public class ActiveWL : BaseLogEntity
    {
        [Column("Looping")]
        public bool Looping { get; set; }

        [Column("Threshold")]
        public byte ThresholdBits { get; set; }

        [Column("Channels")]
        public byte ChannelCount {  get; set; }


    }
}

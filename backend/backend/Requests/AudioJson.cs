using backend.Models;

namespace backend.Requests
{
    public class AudioJson
    {
       
        public UInt16 SampleRate { get; set; }

        public byte ThresholdBits { get; set; }

        public byte ChannelCount { get; set; }

        public UInt32 BlockCountPerChannel { get; set; }

        public List<byte>? AudioData { get; set; }

        public int WLC_ID { get; set; }



    }
}

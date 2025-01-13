using backend.Models;

namespace backend.Requests
{
    public class AudioJson
    {
       
        public UInt16 SampleRate { get; set; }

        //Same as before. 
        public byte ThresholdBits { get; set; }

        public byte ChannelCount { get; set; }

        //Total block count / ChannelCount.
        public UInt32 BlockCountPerChannel { get; set; }

        //Serialized data.
        public List<byte>? AudioData { get; set; }

        public int WL_ID { get; set; }



    }
}

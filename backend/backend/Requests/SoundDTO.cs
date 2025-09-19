namespace backend.Requests
{
    public class SoundDTO
    {
        //Complete WAV data (incl. Header). Obtain sample rate from here.
        public string? SoundData { get; set; }

        //SHA-256 Encoded.
        public string? SoundHash { get; set; }

        //Left shift 1 by the value below to get the minimum and maximum values a sample is to be limited to.
        public byte ThresholdBits { get; set; }

        //Number of channels in the source audio. Limited to 255.
        public byte ChannelCount { get; set; }

        //If checked, audio will be looping, otherwise it will be a one-shot. 
        public bool Looping { get; set; }

    }
}

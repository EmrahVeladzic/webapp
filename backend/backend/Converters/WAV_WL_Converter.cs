using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using System.Collections.Generic;

namespace backend.Converters
{
    public class SFX_DATA
    {
        public AudioJson? Audio {  get; set; }

        public WL? Output { get; set; }

        public WAV? Sound { get; set; }

        private SoundJson? Input { get; set; }

        public byte GetShift(Int16[] input)
        {
            byte shift = 0;
            bool repeat = true;

            while (repeat)
            {
                repeat = false;

                int divisor = 1 << shift;

                foreach (Int16 value in input)
                {
                    int temp = value / divisor;

                    if (temp < -8 || temp > 7)
                    {
                        repeat = true;
                        break;
                    }
                }

                if (repeat)
                {
                    shift++;
                }
            }

            return shift;
        }


        public void EncodeBlock(int blockID)
        {
            List<Int16> tSamples = new List<Int16>();

            for (int i = 0; i < 28; i++)
            {
                tSamples.Add((Int16)(this.Sound!.Data![(28*blockID) + i]));
            }

            ADPCMBlock block = new ADPCMBlock();

            block.Shift_Filter = (byte)(GetShift(tSamples.ToArray())<<4);

            if (Input!.Looping && (this.Output!.BlockCountPerChannel*this.Output.ChannelCount)>=2)
            {
                block.Flags = 2;
            }
            else
            {
                block.Flags = 0;
            }

            block.Samples = new List<byte>();

            int divisor = 1 << (block.Shift_Filter >> 4);
            for (int i = 0; i < 28; i += 2)
            {
                byte sample1 = (byte)((tSamples[i] / divisor) & 0x0F);
                byte sample2 = (byte)((tSamples[i + 1] / divisor) & 0x0F);

                block.Samples.Add((byte)((sample1 << 4) | sample2));
            }


            this.Output!.Data!.Add(block);
        }


        public SFX_DATA(SoundJson input)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            this.Input = input;

            this.Sound = ctx.WAVs.Where(w => w.Hash == this.Input.SoundHash).First();

            this.Sound.Setup(this.Sound!.Serialized!,this.Sound!.Hash!);

            Int16 Threshold = (Int16)(1 << this.Input.ThresholdBits);


            for (int i = 0; i < this.Sound!.Data!.Count; i++)
            {

                if (i >= this.Input!.ChannelCount)
                {
                    this.Sound!.Data![i]-=this.Sound!.Data![(i-this.Input!.ChannelCount)];
                }



                if (this.Sound!.Data[i] > (Int16)(Threshold))
                {
                    this.Sound!.Data[i] = (Int16)(Threshold);
                }
                else if (this.Sound?.Data[i] < (Int16)(-Threshold))
                {
                    this.Sound!.Data[i] = (Int16)(-Threshold);
                }


            }


            int remainingSamples = this.Sound.Data.Count % (this.Input!.ChannelCount * 28);
            if (remainingSamples > 0)
            {
                int padding = (this.Input!.ChannelCount * 28) - remainingSamples;
                this.Sound.Data.AddRange(Enumerable.Repeat((Int16)0, padding));
            }


            this.Output = new WL();

            this.Output.SampleRate = (UInt16)this.Sound.SampleRate;

            this.Output.SerializedSampleRate = (Int16)(this.Sound.SampleRate);

            this.Output.ThresholdBits = this.Input.ThresholdBits;

            this.Output.ChannelCount = this.Input.ChannelCount;

            this.Output.BlockCountPerChannel = this.Sound!.Data!.Count / (Int32)(this.Output.ChannelCount * 28);


            for (int i = 0; i < this.Sound!.Data.Count / 28; i++)
            {

                EncodeBlock(i);

            }

            if (Input!.Looping && (this.Output!.BlockCountPerChannel * this.Output.ChannelCount) >= 2)
            {
                this.Output!.Data!.First().Flags = 6;
                this.Output.Data!.Last().Flags = 3;
            }


            this.Audio = new AudioJson();


            this.Output.Serialize();


            ctx.WLs.Add(this.Output);            

            ctx.SaveChanges();

            ctx.Dispose();

            this.Audio.AudioData = new List<byte>(this.Output!.Serialized!);
          

            this.Audio.SampleRate = this.Output!.SampleRate;

            this.Audio.ThresholdBits = this.Output!.ThresholdBits;

            this.Audio.ChannelCount = this.Output!.ChannelCount;

            this.Audio.BlockCountPerChannel = (UInt32)this.Output.BlockCountPerChannel;

            this.Audio.WL_ID = this.Output!.ID;

            

        }

    }
}

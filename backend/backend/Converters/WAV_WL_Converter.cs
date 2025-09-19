using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace backend.Converters
{
    public class SFX_DATA :BaseConverter
    {

        public WL? Output { get; set; }

        public WAV? Sound { get; set; }

        private Int16? OldSample = null;
        private Int16? OlderSample = null;

        private SoundDTO? Input { get; set; }

        public double GetBlockDistance(Int16[] Original, Int16[] Filtered)
        {
            double DistanceSum = 0.0;

            for (int i = 0; i < 28; i++)
            {
                double diff = (double)(Filtered[i] - Original[i]);
                DistanceSum += (diff*diff);

            }

            return Math.Sqrt(DistanceSum);
        }

        public Int16[] FilterBlock(Int16[] Input, byte filter_select)
        {
            Int16[] Output = (Int16[])Input.Clone();

            switch (filter_select)
            {
                
                case 1:{

                        if (OldSample != null)
                        {
                            Output[0] += (Int16)((((Int32)OldSample * 60) + 32) / 64);
                        }

                        for (int i = 1; i < 28; i++)
                        {
                            Output[i] += (Int16)((((Int32)Output[i - 1] * 60) + 32) / 64);
                        }
                        break;
                }
                case 2:{

                        if (OldSample != null&&OlderSample!=null)
                        {
                            Output[0] += (Int16)(((((Int32)OldSample * 115) - ((Int32)OlderSample * 52)) + 32) / 64);
                        }

                        for(int i = 2; i<28; i++)
                        {

                            Output[i] += (Int16)(((((Int32)Output[i-1] * 115) - ((Int32)Output[i-2] * 52)) + 32) / 64);

                        }

                        break;
                }
                case 3:{

                        if (OldSample != null && OlderSample != null)
                        {
                            Output[0] += (Int16)(((((Int32)OldSample * 98) - ((Int32)OlderSample * 55)) + 32) / 64);
                        }

                        for (int i = 2; i < 28; i++)
                        {

                            Output[i] += (Int16)(((((Int32)Output[i - 1] * 98) - ((Int32)Output[i - 2] * 55)) + 32) / 64);

                        }

                        break;
                }
                case 4:{

                        if (OldSample != null && OlderSample != null)
                        {
                            Output[0] += (Int16)(((((Int32)OldSample * 122) - ((Int32)OlderSample * 60)) + 32) / 64);
                        }

                        for (int i = 2; i < 28; i++)
                        {

                            Output[i] += (Int16)(((((Int32)Output[i - 1] * 122) - ((Int32)Output[i - 2] * 60)) + 32) / 64);

                        }

                        break;
                }
                default:{
                        break;
                }


            }

            return Output;
        }

        public byte GetFilter(Int16[] Original, Int16[] BlockData)
        {
            byte filter_select = 0;

            Int16[] temp = (Int16[])BlockData.Clone();

            double distance = GetBlockDistance(Original, temp);

            for(byte i = 1; i<5; i++)
            {
                temp = FilterBlock(BlockData, i);
                double d = GetBlockDistance(Original, temp);
                if (d < distance)
                {
                    distance = d;
                    filter_select = i;
                }

            }

            temp = FilterBlock(BlockData, filter_select);

            OlderSample = temp[26];
            OldSample = temp[27];

            return (byte)((filter_select&0x7)<<4);
        }

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

            return (byte)(12 - shift);
        }


        public void EncodeBlock(int blockID)
        {
            List<Int16> tSamples = new();

            Int16 Threshold = (Int16)(((1<<this.Input!.ThresholdBits)/2)-1);

            for (int i = 0; i < 28; i++)
            {
                tSamples.Add((Int16)(this.Sound!.Data![(28*blockID) + i]));
            }

            List<Int16> tSamplesOriginal = tSamples.ToList();

            if (blockID > 0)
            {
                Int16 prev = (Int16)(this.Sound!.Data![(28*(blockID-1))+27]);
                tSamples[0]-=prev;
            }

            for (int i = 1; i < 28; i++)
            {
                tSamples[i] -= tSamples[i - 1];
            }


            Int16 LargestAbsolute = (Int16)tSamples.Select(x => Math.Min(Math.Abs((int)x), (int)Int16.MaxValue)).Max();
            if (LargestAbsolute > Threshold)
            {
                float reduction = (float)Threshold/(float)LargestAbsolute;
                tSamples = tSamples.ConvertAll(x => (Int16)Math.Round((float)x * reduction));
            }



            ADPCMBlock block = new();

            block.Shift_Filter = (byte)(GetShift(tSamples.ToArray()));

         
            block.Flags = 0;
            

            block.Samples = new();

            int divisor = 1 << (12 - block.Shift_Filter);

            List<Int16> bData = new();

            for (int i = 0; i < 28; i += 2)
            {
                byte sample1 = (byte)((tSamples[i] / divisor) & 0x0F);
                byte sample2 = (byte)((tSamples[i + 1] / divisor) & 0x0F);

                block.Samples.Add((byte)((sample1 << 4) | sample2));

                Int16 samp = (Int16)sample1;
                if (samp > 7)
                {
                    samp -= 16;
                }
                samp *= (Int16)divisor;
                bData.Add(samp);

                samp = (Int16)sample2;
                if (samp > 7)
                {
                    samp -= 16;
                }
                samp *= (Int16)divisor;
                bData.Add(samp);

            }

            block.Shift_Filter |= GetFilter(tSamplesOriginal.ToArray(),bData.ToArray());

            if (((blockID + 1) * (int)this.Input.ChannelCount) % (this.Sound!.Data!.Count / 28) == 0)
            {
                OlderSample = null;
                OldSample = null;
            }


            this.Output!.Data!.Add(block);
        }

        public SFX_DATA() { }

        public SFX_DATA(SoundDTO input)
        {


            this.Input = input;

        }

        public override async Task Convert(DarkforgeDBContext ctx) { 

            this.Sound = await ctx.WAVs.Where(w => w.Hash == this.Input!.SoundHash).FirstAsync();

            int remainingSamples = this.Sound!.Data!.Count % (this.Input!.ChannelCount * 28);
            if (remainingSamples > 0)
            {
                int padding = (this.Input!.ChannelCount * 28) - remainingSamples;
                this.Sound.Data.AddRange(Enumerable.Repeat((Int16)0, padding));
            }
            if (!Input!.Looping)
            {
                this.Sound.Data.AddRange(Enumerable.Repeat((Int16)0, (this.Input!.ChannelCount * 28)));
            }


            if (this.Sound.ChannelCount > 1)
            {


                Int16[] temp = new Int16[this.Sound!.Data.Count];

                int index = 0;
                for (int i = 0; i < this.Sound.ChannelCount; i++)
                {

                    for (int j = i; j < this.Sound.Data.Count; j += this.Sound.ChannelCount)
                    {
                        temp[index] = this.Sound.Data[j];
                        index++;
                    }

                }


                this.Sound.Data = temp.ToList();

            }

            this.Output = new();

            this.Output.SampleRate = (UInt16)this.Sound.SampleRate;

            this.Output.SerializedSampleRate = (Int16)(this.Sound.SampleRate);

            this.Output.ThresholdBits = this.Input.ThresholdBits;

            this.Output.ChannelCount = this.Input.ChannelCount;

            this.Output.BlockCountPerChannel = this.Sound!.Data!.Count / (Int32)(this.Output.ChannelCount * 28);


            for (int i = 0; i < this.Sound!.Data.Count / 28; i++)
            {
                EncodeBlock(i);
            }

            if (this.Output.BlockCountPerChannel > 0)
            {
                if (Input!.Looping)
                {
                    for (byte i = 0; i < this.Input.ChannelCount; i++)
                    {
                        this.Output!.Data![((int)i * this.Output.BlockCountPerChannel)].Flags = 6;
                        this.Output!.Data![((int)i * this.Output.BlockCountPerChannel) + (this.Output.BlockCountPerChannel - 1)].Flags = 3;
                    }

                }
                else
                {
                    for (byte i = 0; i < this.Input.ChannelCount; i++)
                    {
                        this.Output!.Data![((int)i * this.Output.BlockCountPerChannel) + (this.Output.BlockCountPerChannel - 1)].Flags = 5;
                    }
                }
            }

            this.Output.Serialize();


            await ctx.WLs.AddAsync(this.Output);            

            await ctx.SaveChangesAsync();


          
        }

    }
}

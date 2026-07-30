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

        private Int16 OldSample = 0;
        private Int16 OlderSample = 0;

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

        
        public Int16[] ShiftBlockDown(Int16[] input, byte shift_bits)
        {
            Int16[] Output = (Int16[])input.Clone();

            for (int j = 0; j < 28; j++)
            {
                int n = Output[j] / (1 << shift_bits);
               
                Output[j] = (Int16)Math.Clamp(n,-8,7);
            }

            return Output;
        }

        public Int16[] ShiftBlockUp(Int16[] input, byte shift_bits)
        {
            Int16[] Output = new Int16[28];
            for (int j = 0; j < 28; j++)
            {
                Output[j] = (Int16)(input[j] * (1 << shift_bits));
            }
                return Output;
        }

        public Int16[] FilterBlock(Int16[] Input, byte filter_select)
        {
            Int16[] Output = (Int16[])Input.Clone();

    
            switch (filter_select)
            {
                
                case 1:{

                     
                        for (int i = 0; i < 28; i++)
                        {
                            Int32 acc = (Int32)Output[i]+ (((((Int32)OldSample*60) + 32)) >> 6);
                            Output[i] = (Int16)Math.Clamp(acc, -32768, 32767);

                            OlderSample = OldSample;
                            OldSample = Output[i];
                        }
                        break;
                }
                case 2:{

                        
                       

                        for(int i = 0; i<28; i++)
                        {

                            Int32 acc = (Int32)Output[i] + (((((Int32)OldSample * 115) - ((Int32)OlderSample * 52)) + 32) >> 6);
                            Output[i] = (Int16)Math.Clamp(acc, -32768, 32767);


                            OlderSample = OldSample;
                            OldSample = Output[i];
                        }

                        break;
                }
                case 3:{

                        
                       

                        for (int i = 0; i < 28; i++)
                        {

                            Int32 acc = (Int32)Output[i] + (((((Int32)OldSample * 98) - ((Int32)OlderSample * 55)) + 32) >> 6);
                            Output[i] = (Int16)Math.Clamp(acc, -32768, 32767);

                            OlderSample = OldSample;
                            OldSample = Output[i];

                        }

                        break;
                }
                case 4:{

                        
                      

                        for (int i = 0; i < 28; i++)
                        {

                            Int32 acc = (Int32)Output[i] + (((((Int32)OldSample * 122) - ((Int32)OlderSample * 60)) + 32) >> 6);
                            Output[i] = (Int16)Math.Clamp(acc, -32768, 32767);

                            OlderSample = OldSample;
                            OldSample = Output[i];

                        }

                        break;
                }
                default:{

                        OlderSample = Output[26];
                        OldSample = Output[27];
      

                        break;
                }


            }

            return Output;
        }

        public byte GetShiftFilter(Int16[] Original, Int16[] diff)
        {
            byte select = 0;

            int sh = 0;
            int fl = 0;

            double distance = double.PositiveInfinity;

            Int16 o = OldSample;
            Int16 oo = OlderSample;

            Int16 wo = 0;
            Int16 woo = 0;

            for (int i = 0;i<13; i++)
            {

                Int16[] shifted = (Int16[])diff.Clone();

                
                shifted = ShiftBlockDown(shifted, (byte)i);
                shifted = ShiftBlockUp(shifted, (byte)i);
                


                for (int j = 0; j<5; j++)
                {
                    Int16[] filtered = (Int16[])shifted.Clone();
                    OldSample = o;
                    OlderSample = oo;
                    filtered = FilterBlock(filtered, (byte)j);

                    double dist = GetBlockDistance(Original, filtered);
                    if(dist<distance)
                    {
                        distance = dist;
                        sh = i;
                        fl = j;

                        wo = OldSample;
                        woo = OlderSample;
                    }
                }

            }

            OldSample = wo;
            OlderSample = woo;

            select = (byte)(((fl & 0x7) << 4) | ((12 - sh) & 0xF));

            return select;
        }
        public void EncodeBlock(int blockID, bool looping)
        {
            List<Int16> tSamples = new();

            Int16 Threshold = (Int16)(((1 << this.Input!.ThresholdBits) / 2) - 1);

            for (int i = 0; i < 28; i++)
            {
                tSamples.Add((Int16)(this.Sound!.Data![(28 * blockID) + i]));
            }

            List<Int16> tSamplesOriginal = tSamples.ToList();

                          
            tSamples[0] -= OldSample;
            

            for (int i = 1; i < 28; i++)
            {
                tSamples[i] -= tSamples[i - 1];
            }

            Int16 LargestAbsolute = (Int16)tSamples.Select(x => Math.Min(Math.Abs((int)x), (int)Int16.MaxValue)).Max();
            if (LargestAbsolute > Threshold)
            {
                float reduction = (float)Threshold / (float)LargestAbsolute;
                tSamples = tSamples.ConvertAll(x => (Int16)Math.Round((float)x * reduction));
            }

            ADPCMBlock block = new();

            block.Shift_Filter = GetShiftFilter(tSamplesOriginal.ToArray(), tSamples.ToArray());

            block.Flags = (byte)(looping? 0x2 : 0x0);

            if (this.Output?.BlockCountPerChannel>0 && blockID%this.Output?.BlockCountPerChannel==0)
            {
                block.Flags|= (byte)(0x4);
            }

            if(this.Output?.BlockCountPerChannel>0 && blockID%this.Output?.BlockCountPerChannel == (this.Output?.BlockCountPerChannel - 1))
            {
                block.Flags |= (byte)(0x1);
            }

           
            byte exp = (byte)(12 - (block.Shift_Filter & 0x0F));

            Int16[] q = ShiftBlockDown(tSamples.ToArray(), exp);

            block.Samples = new();

            for (int i = 0; i < 28; i += 2)
            {
                byte lo = (byte)(q[i] & 0x0F);   
                byte hi = (byte)(q[i + 1] & 0x0F);   

                block.Samples.Add((byte)((hi << 4) | lo));
            }

            this.Output!.Data!.Add(block);
        }

        public int AddDummyBlocks(bool looping)
        {
            int n = this.Output?.BlockCountPerChannel ?? 0;
            int output = (4 - (n & 0x3)) & 0x3;

            if (output==0 && !looping) { output = 4; }

            for (int i = 0; i< output; i++)
            {
                ADPCMBlock block = new();
                block.Shift_Filter = 0;

                block.Samples = new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                
                block.Flags = 0x2;

                if (i == 0) { block.Flags |= 0x4; }

                if (i == output - 1) {  block.Flags |= 0x1; }
                
                this?.Output?.Data?.Add(block);

            }


            return output;
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

            int add = 0;

            for (int i = 0; i < (int)this.Output.ChannelCount; i++)
            {
                OldSample = 0;
                OlderSample = 0;

                for (int j = 0; j < this.Output.BlockCountPerChannel; j++)
                {
                    EncodeBlock((i * this.Output.BlockCountPerChannel) + j, this.Input.Looping);
                }

                add = AddDummyBlocks(this.Input.Looping);
            }

            this.Output.BlockCountPerChannel += add;

            this.Output.Serialize();


            await ctx.WLs.AddAsync(this.Output);            

            await ctx.SaveChangesAsync();


          
        }

    }
}

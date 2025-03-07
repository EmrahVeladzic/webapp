using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using backend.Converters;
using backend.Utils;

namespace backend.Models
{
    [Table("WL", Schema = "Models")]
    public class WL:BaseBufferEntity, ITopLevelModel
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

        public WL():base()
        {
            this.Data = new List<ADPCMBlock>();
            this.ToSerialize= new List<byte> { };
        }

        public override void Serialize()
        {
            foreach(ADPCMBlock block in this.Data!)
            {
                block.Serialize(this.ToSerialize!);   
            }

            this.Serialized = this.ToSerialize!.ToArray();
        }


        public override void Deserialize()
        {
            this.SampleRate = (UInt16)this.SerializedSampleRate;

            for (int i = 0; i <Serialized!.Length; i+=16)
            {
                ADPCMBlock temp = new ADPCMBlock();

                temp.Deserialize(this.Serialized!, i);

                this.Data!.Add(temp);
            }

            
        }


        public byte[] ToArrayBuffer()
        {
            List<byte> temp = new List<byte>();

            temp.Add(87);
            temp.Add(ChannelCount);
            temp.Add(ThresholdBits);
            PrimitiveSerialization.SerializePrimitive((UInt32)this.BlockCountPerChannel,temp);
            PrimitiveSerialization.SerializePrimitive(this.SampleRate,temp);
            temp.AddRange(this.Serialized!);

            byte[] data = temp.ToArray();
            temp.Clear();
            return data;
        }

        public override void Clear()
        {
            this.Data?.Clear();
            this.ToSerialize?.Clear();
            this.Serialized = null;
        }

    }
}

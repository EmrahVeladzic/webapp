using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("WAV", Schema = "Files")]
    public class WAV
    {
        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        [Column("EntityHash")]
        public string? Hash { get; set; }

        [NotMapped]
        public UInt32 Magic { get; set; }

        [NotMapped]
        public UInt32 FileSize { get; set; }

        [NotMapped]
        public UInt32 FileType { get; set; }

        [NotMapped]
        public UInt32 FMTMarker { get; set; }

        [NotMapped]
        public UInt32 FMTSize { get; set; }

        [NotMapped]
        public UInt16 Format { get; set; }

        [NotMapped]
        public UInt16 ChannelCount { get; set; }

        [NotMapped]
        public UInt32 SampleRate { get; set; }

        [NotMapped]
        public UInt32 ByteRate { get; set; }

        [NotMapped]
        public UInt16 BlockAlignment { get; set; }

        [NotMapped]
        public UInt16 BitsPerSample { get; set; }

        [NotMapped]
        public UInt32 DataMarker { get; set; }

        [NotMapped]
        public UInt32 DataSectionSize { get; set; }

        [NotMapped]
        public List<Int16>? Data { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

        public WAV()
        {

        }


        public void Setup(byte[] Input, string hash)
        {

            this.Hash = hash;

            this.Magic = BitConverter.ToUInt32(Input,0);

            this.FileSize = BitConverter.ToUInt32(Input, 4);

            this.FileType = BitConverter.ToUInt32(Input, 8);

            this.FMTMarker = BitConverter.ToUInt32(Input, 12);

            this.FMTSize = BitConverter.ToUInt32(Input, 16);

            this.Format = BitConverter.ToUInt16(Input, 20);

            this.ChannelCount = BitConverter.ToUInt16(Input, 22);

            this.SampleRate = BitConverter.ToUInt32(Input, 24);

            this.ByteRate = BitConverter.ToUInt32(Input, 28);

            this.BlockAlignment = BitConverter.ToUInt16(Input, 32);

            this.BitsPerSample = BitConverter.ToUInt16(Input, 34);

            this.DataMarker = BitConverter.ToUInt32(Input, 36);

            this.DataSectionSize = BitConverter.ToUInt32(Input, 40);

            this.Data = new List<Int16>();

            for (int i = 0; i < DataSectionSize/sizeof(Int16); i++)
            {
                this.Data.Add(BitConverter.ToInt16(Input,44 + (i*sizeof(Int16))));
            }

            this.Serialized = Input;

        }

    }
}

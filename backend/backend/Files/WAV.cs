using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Files
{
    [Table("WAV", Schema = "Files")]
    public class WAV:BaseFileEntity
    {
     
        [NotMapped]
        public uint Magic { get; set; }

        [NotMapped]
        public uint FileSize { get; set; }

        [NotMapped]
        public uint FileType { get; set; }

        [NotMapped]
        public uint FMTMarker { get; set; }

        [NotMapped]
        public uint FMTSize { get; set; }

        [NotMapped]
        public ushort Format { get; set; }

        [NotMapped]
        public ushort ChannelCount { get; set; }

        [NotMapped]
        public uint SampleRate { get; set; }

        [NotMapped]
        public uint ByteRate { get; set; }

        [NotMapped]
        public ushort BlockAlignment { get; set; }

        [NotMapped]
        public ushort BitsPerSample { get; set; }

        [NotMapped]
        public uint DataMarker { get; set; }

        [NotMapped]
        public uint DataSectionSize { get; set; }

        [NotMapped]
        public List<short>? Data { get; set; }
             
        public WAV()
        {

        }

        public override void Destructor()
        {
            base.Destructor();
            this.Data!.Clear();
        }


        public override void Setup(byte[] Input, string hash)
        {

            Hash = hash;

            Magic = BitConverter.ToUInt32(Input, 0);

            FileSize = BitConverter.ToUInt32(Input, 4);

            FileType = BitConverter.ToUInt32(Input, 8);

            FMTMarker = BitConverter.ToUInt32(Input, 12);

            FMTSize = BitConverter.ToUInt32(Input, 16);

            Format = BitConverter.ToUInt16(Input, 20);

            ChannelCount = BitConverter.ToUInt16(Input, 22);

            SampleRate = BitConverter.ToUInt32(Input, 24);

            ByteRate = BitConverter.ToUInt32(Input, 28);

            BlockAlignment = BitConverter.ToUInt16(Input, 32);

            BitsPerSample = BitConverter.ToUInt16(Input, 34);

            DataMarker = BitConverter.ToUInt32(Input, 36);

            DataSectionSize = BitConverter.ToUInt32(Input, 40);

            Data = new List<short>();

            for (int i = 0; i < DataSectionSize / sizeof(short); i++)
            {
                Data.Add(BitConverter.ToInt16(Input, 44 + i * sizeof(short)));
            }

            Serialized = Input;

        }

    }
}

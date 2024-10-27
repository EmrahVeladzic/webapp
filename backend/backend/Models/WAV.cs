using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("BMP", Schema = "Files")]
    public class WAV
    {
        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        [Column("EntityHash")]
        public string? Hash { get; set; }

        public UInt32 Magic {  get; set; }

        public UInt32 FileSize { get; set; }

        public UInt32 FileType { get; set; }

        public UInt32 FMTMarker { get; set; }

        public UInt32 FMTSize { get; set; }

        public UInt16 Format { get; set; }

        public UInt16 ChannelCount { get; set; }

        public UInt32 SampleRate { get; set; }

        public UInt32 ByteRate { get; set; }

        public UInt16 BlockAlignment { get; set; }

        public UInt16 BitsPerSample { get; set; }

        public UInt32 DataMarker { get; set; }

        public UInt32 DataSectionSize { get; set; }

        public List<Int16>? Data {  get; set; }

        [Column("EntityData")]
        public byte[]? Serialized { get; set; }

    }
}

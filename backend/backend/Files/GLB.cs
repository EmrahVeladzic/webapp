using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace backend.Files
{
    [Table("GLB", Schema = "Files")]
    public class GLB:BaseFileEntity
    {

        [NotMapped]
        public string? GLTF_Header { get; set; }

        [NotMapped]
        public uint GLTF_Version { get; set; }


        [NotMapped]
        public uint GLTF_Length { get; set; }


        [NotMapped]
        public uint Metadata_Length { get; set; }

        [NotMapped]
        public string? Metadata_Header { get; set; }


        [NotMapped]
        public uint BLOB_Length { get; set; }

        [NotMapped]
        public string? BLOB_Header { get; set; }



        [NotMapped]
        public string? Metadata_String { get; set; }


        [NotMapped]
        public JsonDocument? Metadata { get; set; }



        [NotMapped]
        public byte[]? BLOB { get; set; }



        public GLB()
        {

        }

        public override void Destructor()
        {
            base.Destructor();
            this.Metadata = null;
            this.BLOB = null;
            this.Metadata_String = null;
        }

        public override void Setup(byte[] Input, string hash)
        {

            Hash = hash;

            GLTF_Header = BitConverter.ToString(Input, 0, 4);

            GLTF_Version = BitConverter.ToUInt32(Input, 4);

            GLTF_Length = BitConverter.ToUInt32(Input, 8);

            Metadata_Length = BitConverter.ToUInt32(Input, 12);

            Metadata_Header = BitConverter.ToString(Input, 16, 4);

            Metadata_String = Encoding.UTF8.GetString(Input, 20, (int)Metadata_Length);

            Metadata = JsonDocument.Parse(Metadata_String);

            BLOB_Length = BitConverter.ToUInt32(Input, (int)(Metadata_Length + 20));

            BLOB_Header = BitConverter.ToString(Input, (int)(Metadata_Length + 24), 4);

            BLOB = new byte[BLOB_Length];

            Array.Copy(Input, (int)(Metadata_Length + 28), BLOB!, 0, BLOB_Length);

            Serialized = Input;
        }

    }
}

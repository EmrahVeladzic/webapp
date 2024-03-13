using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace backend.Models
{
    [Table("BMP")]
    public class BMP
    {
        [Key]
        [Column("Id")]
        public UInt32 Id { get; set; }

        [Column("Hash")]
        public string? Hash { get; set; }

        [Column("Magic")]
        public UInt16 Magic { get; set; }

        [Column("FileSize")]
        public UInt32 FileSize { get; set; }

        [Column("Reserved")]
        public UInt32 Reserved { get; set; }

        [Column("Offset")]
        public UInt32 Offset { get; set; }

        [Column("HeaderSize")]
        public UInt32 HeaderSize { get; set; }

        [Column("Width")]
        public Int32 Width { get; set; }

        [Column("Height")]
        public Int32 Height { get; set; }

        [Column("Planes")]
        public UInt16 Planes { get; set; }

        [Column("BPP")]
        public UInt16 BPerPixel {  get; set; }

        [Column("Compression")]
        public UInt32 Compression {  get; set; }

        [Column("ImgSize")]
        public UInt32 ImgSize { get; set; }

        [Column("XPixelPerm")]
        public Int32 XPixelPerm { get; set; }

        [Column("YPixelPerm")]
        public Int32 YPixelPerm { get; set; }

        [Column("ColoursUsed")]
        public UInt32 ColoursUsed { get; set; }

        [Column("ImportantColours")]
        public UInt32 ImportantColours { get; set; }

        [Column("Data")]
        public List<byte>? Serialized {  get; set; }

        [NotMapped]
        public List<Pixel24>? Data {  get; set; }

        public BMP()
        {
           
        }
        public void Setup(byte[] Input, string sha1)
        {

           this.Hash = sha1;

           this.Magic = BitConverter.ToUInt16(Input,0);
          
           this.FileSize = BitConverter.ToUInt32(Input,2);
           this.Reserved = BitConverter.ToUInt32(Input,6);
           this.Offset = BitConverter.ToUInt32(Input,10);
           this.HeaderSize = BitConverter.ToUInt32(Input,14);
          
           this.Width = BitConverter.ToInt32(Input,18);
           this.Height = BitConverter.ToInt32(Input,22);
          
           this.Planes = BitConverter.ToUInt16(Input,24);
           this.BPerPixel = BitConverter.ToUInt16(Input,26);
          
           this.Compression = BitConverter.ToUInt32(Input,30);
           this.ImgSize = BitConverter.ToUInt32(Input,34);
          
           this.XPixelPerm = BitConverter.ToInt32(Input,38);
           this.YPixelPerm = BitConverter.ToInt32(Input,42);
           
           this.ColoursUsed = BitConverter.ToUInt32(Input,46);
           this.ImportantColours = BitConverter.ToUInt32(Input,50);

            this.Data = new List<Pixel24>();

            this.Serialized = new List<byte>();

            Pixel24 Temp = new Pixel24();

           

            for (int i = 0; i < (this.Width*this.Height); i++)
            {               

                Temp.Blue = Input[54 + (i * 3) + 0];
                Temp.Green = Input[54 + (i * 3) + 1];
                Temp.Red = Input[54 + (i * 3) + 2];                

                this.Data.Add(Temp);

                this.Serialized.Add(Temp.Blue);
                this.Serialized.Add(Temp.Green);
                this.Serialized.Add(Temp.Red);
            }

           
        }

        public void Deserialize_Data()
        {
            this.Data= new List<Pixel24>();

            Pixel24 Temp = new Pixel24();

            for (int i = 0;i <this.Serialized!.Count(); i += 3)
            {
                Temp.Blue= this.Serialized![i];
                Temp.Green = this.Serialized![i+1];
                Temp.Red = this.Serialized![i+2];
                
            }


        }


    }
}

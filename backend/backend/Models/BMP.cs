using backend.Database;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace backend.Models
{
    [Table("BMP",Schema ="Files")]
    public class BMP
    {
        [Key]
        [Column("EntityID")]
        public int ID { get; set; }

        [Column("EntityHash")]
        public string? Hash { get; set; }

        [NotMapped]
        public UInt16 Magic { get; set; }

        [NotMapped]
        public UInt32 FileSize { get; set; }

        [NotMapped]
        public UInt32 Reserved { get; set; }

        [NotMapped]
        public UInt32 Offset { get; set; }

        [NotMapped]
        public UInt32 HeaderSize { get; set; }

        [NotMapped]
        public Int32 Width { get; set; }

        [NotMapped]
        public Int32 Height { get; set; }

        [NotMapped]
        public UInt16 Planes { get; set; }

        [NotMapped]
        public UInt16 BPerPixel {  get; set; }

        [NotMapped]
        public UInt32 Compression {  get; set; }

        [NotMapped]
        public UInt32 ImgSize { get; set; }

        [NotMapped]
        public Int32 XPixelPerm { get; set; }

        [NotMapped]
        public Int32 YPixelPerm { get; set; }

        [NotMapped]
        public UInt32 ColoursUsed { get; set; }

        [NotMapped]
        public UInt32 ImportantColours { get; set; }

        [Column("EntityData")]
        public byte[]? Serialized {  get; set; }

        [NotMapped]
        public List<Pixel24>? Data {  get; set; }

        public BMP()
        {
           
        }

        
        
        public void Setup(byte[] Input, string hash)
        {

            this.Hash = hash;

            this.Magic = BitConverter.ToUInt16(Input,0);
          
            this.FileSize = BitConverter.ToUInt32(Input,2);
            this.Reserved = BitConverter.ToUInt32(Input,6);
            this.Offset = BitConverter.ToUInt32(Input,10);
            this.HeaderSize = BitConverter.ToUInt32(Input,14);
          
            this.Width = BitConverter.ToInt32(Input,18);
            this.Height = BitConverter.ToInt32(Input,22);
          
            this.Planes = BitConverter.ToUInt16(Input,26);
            this.BPerPixel = BitConverter.ToUInt16(Input,28);
          
            this.Compression = BitConverter.ToUInt32(Input,30);
            this.ImgSize = BitConverter.ToUInt32(Input,34);
            
            this.XPixelPerm = BitConverter.ToInt32(Input,38);
            this.YPixelPerm = BitConverter.ToInt32(Input,42);
           
            this.ColoursUsed = BitConverter.ToUInt32(Input,46);
            this.ImportantColours = BitConverter.ToUInt32(Input,50);

            this.Data = new List<Pixel24>();            
            

            int Row_Size = this.Width*3;

            for (int i = 1; i < 4; i++)
            {
                if (((Row_Size + i) % 4)== 0){

                    Row_Size += i;
                    break;

                }
            }

            for (int i = this.Height-1; i >= 0; i--)
            {
                int Row_Start =(int)this.Offset + (i * Row_Size);                              

                for (int j = 0; j < this.Width; j++)
                {
                    int Index = Row_Start + (j * 3);

                    this.Data.Add(new Pixel24(Input[Index], Input[Index + 1], Input[Index+2]));

                }
            }
                    
            this.Serialized = Input;
        }
        
    }
}

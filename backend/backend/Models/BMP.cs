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
        

        /*
        public void Setup(byte[] Input, string hash)
        {
            this.Hash = hash;

            this.Magic = BitConverter.ToUInt16(Input, 0);
            this.FileSize = BitConverter.ToUInt32(Input, 2);
            this.Reserved = BitConverter.ToUInt32(Input, 6);
            this.Offset = BitConverter.ToUInt32(Input, 10);
            this.HeaderSize = BitConverter.ToUInt32(Input, 14);
            this.Width = BitConverter.ToInt32(Input, 18);
            this.Height = BitConverter.ToInt32(Input, 22);
            this.Planes = BitConverter.ToUInt16(Input, 26);
            this.BPerPixel = BitConverter.ToUInt16(Input, 28);
            this.Compression = BitConverter.ToUInt32(Input, 30);
            this.ImgSize = BitConverter.ToUInt32(Input, 34);
            this.XPixelPerm = BitConverter.ToInt32(Input, 38);
            this.YPixelPerm = BitConverter.ToInt32(Input, 42);
            this.ColoursUsed = BitConverter.ToUInt32(Input, 46);
            this.ImportantColours = BitConverter.ToUInt32(Input, 50);

            this.Data = new List<Pixel24>();

            int rowSize = ((this.Width * 3 + 3) / 4) * 4; // Row size with padding
            int padding = rowSize - (this.Width * 3); // Calculate padding bytes per row
            int dataOffset = (int)this.Offset;

            Console.WriteLine($"Width: {Width}, Height: {Height}, Row Size: {rowSize}, Padding: {padding}");

            for (int y = 0; y < this.Height; y++)
            {
                int rowStart = dataOffset + ((this.Height - 1 - y) * rowSize); // Start of the row
                for (int x = 0; x < this.Width; x++)
                {
                    int pixelIndex = rowStart + (x * 3); // Each pixel is 3 bytes (BGR format)
                    Pixel24 temp = new Pixel24
                    {
                        Blue = Input[pixelIndex],
                        Green = Input[pixelIndex + 1],
                        Red = Input[pixelIndex + 2]
                    };

                    this.Data.Add(temp);
                }
            }

            Console.WriteLine($"Count of Magenta Pixels: {Data.Count(d => d.Red == 255 && d.Green == 0 && d.Blue == 255)}");

            this.Serialized = Input;
        }*/





    }
}

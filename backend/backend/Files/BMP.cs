using backend.Database;
using backend.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace backend.Files
{
    [Table("BMP", Schema = "Files")]
    public class BMP:BaseFileEntity
    {
       

        [NotMapped]
        public ushort Magic { get; set; }

        [NotMapped]
        public uint FileSize { get; set; }

        [NotMapped]
        public uint Reserved { get; set; }

        [NotMapped]
        public uint Offset { get; set; }

        [NotMapped]
        public uint HeaderSize { get; set; }

        [NotMapped]
        public int Width { get; set; }

        [NotMapped]
        public int Height { get; set; }

        [NotMapped]
        public ushort Planes { get; set; }

        [NotMapped]
        public ushort BPerPixel { get; set; }

        [NotMapped]
        public uint Compression { get; set; }

        [NotMapped]
        public uint ImgSize { get; set; }

        [NotMapped]
        public int XPixelPerm { get; set; }

        [NotMapped]
        public int YPixelPerm { get; set; }

        [NotMapped]
        public uint ColoursUsed { get; set; }

        [NotMapped]
        public uint ImportantColours { get; set; }

        [NotMapped]
        public List<Pixel24>? Data { get; set; }

        public BMP()
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

            Magic = BitConverter.ToUInt16(Input, 0);

            FileSize = BitConverter.ToUInt32(Input, 2);
            Reserved = BitConverter.ToUInt32(Input, 6);
            Offset = BitConverter.ToUInt32(Input, 10);
            HeaderSize = BitConverter.ToUInt32(Input, 14);

            Width = BitConverter.ToInt32(Input, 18);
            Height = BitConverter.ToInt32(Input, 22);

            Planes = BitConverter.ToUInt16(Input, 26);
            BPerPixel = BitConverter.ToUInt16(Input, 28);

            Compression = BitConverter.ToUInt32(Input, 30);
            ImgSize = BitConverter.ToUInt32(Input, 34);

            XPixelPerm = BitConverter.ToInt32(Input, 38);
            YPixelPerm = BitConverter.ToInt32(Input, 42);

            ColoursUsed = BitConverter.ToUInt32(Input, 46);
            ImportantColours = BitConverter.ToUInt32(Input, 50);

            Data = new List<Pixel24>();


            int Row_Size = Width * 3;

            for (int i = 1; i < 4; i++)
            {
                if ((Row_Size + i) % 4 == 0)
                {

                    Row_Size += i;
                    break;

                }
            }

            for (int i = Height - 1; i >= 0; i--)
            {
                int Row_Start = (int)Offset + i * Row_Size;

                for (int j = 0; j < Width; j++)
                {
                    int Index = Row_Start + j * 3;

                    Data.Add(new Pixel24(Input[Index], Input[Index + 1], Input[Index + 2]));

                }
            }

            Serialized = Input;
        }

    }
}

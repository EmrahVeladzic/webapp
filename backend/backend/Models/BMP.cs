using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("BMP")]
    public class BMP
    {
        [Key]
        public UInt32 Id { get; set; }

        public UInt16 Magic { get; set; }

        public UInt32 FileSize { get; set; }
        public UInt32 Reserved { get; set; }
        public UInt32 Offset { get; set; }
        public UInt32 HeaderSize { get; set; }

        public Int32 Width { get; set; }
        public Int32 Height { get; set; }

        public UInt16 Planes { get; set; }
        public UInt16 BPerPixel {  get; set; }
        public UInt32 Compression {  get; set; }

        public UInt32 ImgSize { get; set; }
        public Int32 XPixelPerm { get; set; }
        public Int32 YPixelPerm { get; set; }

        public UInt32 ColoursUsed { get; set; }
        public UInt32 ImportantColours { get; set; }

        public List<Pixel24>? Data {  get; set; }

        
    }
}

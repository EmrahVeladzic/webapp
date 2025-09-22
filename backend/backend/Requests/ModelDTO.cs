using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Requests
{
    public class ModelDTO
    {

        //Complete GLB data (headers + BIN + GLTF)
        public string? ModelData { get; set; }

        //SHA-256 Encoded
        public string? ModelHash { get; set; }

        //Target fixed-point precision
        public byte PrecisionBits { get; set; }

        //Framerate of the target platform
        public byte TargetFPS { get; set; }

        //Target texture dimensions. +1, as 0 is not valid. This does not need authorization to edit
        public byte TexWidth { get; set; }
        public byte TexHeight { get; set; }        

        //Data below is the same as in the RPF, and should not need authorization to edit
        public byte TexPageX { get; set; }
        public byte TexPageY { get; set; }
        public byte TexOffsetX { get; set; }
        public byte TexOffsetY { get; set; }

        //In VRAM indexed textures appear smaller in the X dimension by a factor of 16/tex_bpp
        public byte TexClutByteReduction { get; set; }

    }
}

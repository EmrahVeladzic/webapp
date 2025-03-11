namespace backend.Requests
{
    public class ModelDTO
    {

        //Complete GLB data (headers + BIN + GLTF).
        public string? ModelData { get; set; }

        //SHA-256 Encoded.
        public string? ModelHash { get; set; }

        //Target fixed-point precision.
        public byte PrecisionBits { get; set; }

        //Framerate of the target platform.
        public byte TargetFPS { get; set; }

        //Target texture dimensions. +1, as 0 is not valid.
        public byte TexWidth { get; set; }
        public byte TexHeight { get; set; }

      

    }
}

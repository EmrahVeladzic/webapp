namespace backend.Models
{
    public class ADPCMBlock
    {
        public byte Shift_Filter {  get; set; }

        public byte Flags { get; set; }

        public List<byte>? Samples { get; set; }

    }
}

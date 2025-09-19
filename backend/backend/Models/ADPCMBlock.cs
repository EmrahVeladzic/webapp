namespace backend.Models
{
    public class ADPCMBlock
    {
        public byte Shift_Filter {  get; set; }

        public byte Flags { get; set; }

        public List<byte>? Samples { get; set; }

        public void Serialize(List<byte> output)
        {
            output!.Add(this.Shift_Filter);

            output!.Add(this.Flags);

            for (int j = 0; j < 14; j++)
            {
                output!.Add(this.Samples![j]);
            }

        }

        public ADPCMBlock()
        {
            this.Samples = new();
   
        }

        public void Deserialize(byte[] data, int begin)
        {
            this.Shift_Filter = data[begin];
            this.Flags = data[begin + 1];

            for (int j = (begin+1); j < (begin+15); j++)
            {   
                this.Samples!.Add(data[j]);
            }
        }

    }
}

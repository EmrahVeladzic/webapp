using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace backend.Converters
{

    public class Occurence_Entry
    {
        public Pixel15? Value { get; set; }
        public UInt32 Occurence { get; set; }
    }

    public class Swap_Entry
    {
        public Pixel15? Recipient { get; set; }
        public Pixel15? Donor { get; set; }
    }


    public class IMG_DATA :BaseConverter
    {

        public bool AlphaUsed { get; set; }
        public Pixel24? Alpha { get; set; }
        private Pixel15? Alpha15 { get; set; }

        private ImageDTO? Input { get; set; }

        private byte ProtectedBufferIndex {get;set;}

        private List<Pixel15>? ProtectedBuffer { get; set; }

        private List<Occurence_Entry>? Occurence_Table { get; set; }

        private List<Swap_Entry>? Swap_Table { get; set;}

        public BMP? Image {  get; set; }

        public RPF? Output { get; set; }

        private UInt32 UniqueCount { get; set; }
        private UInt32 MaxUniqueCount { get; set; }
            
        public byte Shift_Value { get; set; }

  

        Vector3 GetHue(Pixel15 input)
        {
            Vector3 output = new Vector3();


            if (input.Red() >= 16)
            {
                output.X = 31.0f;
            }
            else
            {
                output.X = 0.0f;
            }


            if (input.Green() >= 16)
            {
                output.Y = 31.0f;
            }
            else
            {
                output.Y = 0.0f;
            }


            if (input.Blue() >= 16)
            {
                output.Z = 31.0f;
            }
            else
            {
                output.Z = 0.0f;
            }



            return output;
        }



        bool EnsureSingle(Pixel15 a, Pixel15 b)
        {
            if (Alpha15 != null)
            {
                return (!(a.Data==Alpha15.Data) && !(b.Data==Alpha15.Data) && !(a.Data==b.Data));
            }

            else
            {
                return !(a.Data==b.Data);
            }
        }

        bool EnsureDual(Pixel15 a, Pixel15 b)
        {
            bool output = EnsureSingle(a, b);

          
            if (output)
            {

                for (int i = 0; i < Input!.ProtectedBufferSize; i++)
                {
                    if (a.Equals(ProtectedBuffer![i]) || b.Equals(ProtectedBuffer[i]))
                    {                       
                        output = false;
                        break;
                        
                    }
                }

                Vector3 vA = GetHue(a);
                Vector3 vB = GetHue(b);

                if(vA != vB)
                {

                    output = false;
                }

            }

            return output;

        }


        void Protect(Pixel15 recentDonor)
        {
            if (Input!.ProtectedBufferSize > 0)
            {
                ProtectedBuffer![(int)ProtectedBufferIndex] = recentDonor;

                ProtectedBufferIndex++;

                if((uint)ProtectedBufferIndex >= (uint)Input!.ProtectedBufferSize)
                {
                    ProtectedBufferIndex = 0;
                }
            }

        }

        void NewColour(Pixel15 colour)
        {
            Occurence_Entry oc = new Occurence_Entry();

            oc.Value = colour;
            oc.Occurence = 1;

            Occurence_Table!.Add(oc);

        }

        void NewSwap(Pixel15 donor, Pixel15 recipient)
        {
            Swap_Entry sw = new Swap_Entry();

            sw.Donor = donor;
            sw.Recipient = recipient;



            List<Swap_Entry> toUpdate = this.Swap_Table!.Where(swa=>swa.Donor!.Equals(recipient)).ToList();
                       
           
            foreach(Swap_Entry s in toUpdate)
            {
                s.Donor = donor;
            }
            

            Swap_Table!.Add(sw);

            Occurence_Entry ocR = Occurence_Table!.Where(o=>o.Value!.Data==recipient.Data).First();

            Occurence_Entry ocD = Occurence_Table!.Where(o => o.Value!.Data == donor.Data).First();

            ocD.Occurence += ocR.Occurence;

            ocR.Occurence = 0;                              


            Occurence_Table = Occurence_Table!.OrderBy(o=>o.Occurence).ToList();

            UniqueCount--;

        }

        void PopularityCompression( int index)
        {

            Vector3 PotentialRecipient = new Vector3();
            Vector3 PotentialDonor = new Vector3();

            Pixel15 initial = Occurence_Table![index].Value!;
            Pixel15 compare = Occurence_Table![index].Value!;

            double Distance = double.PositiveInfinity;

            PotentialRecipient.X = (float)initial.Red();
            PotentialRecipient.Y = (float)initial.Green();
            PotentialRecipient.Z = (float)initial.Blue();

            int chosen = 0;

            for (int i = (int)(MaxUniqueCount-UniqueCount); i< (int)MaxUniqueCount ; i++)
            {
                compare = Occurence_Table![i].Value!;

                PotentialDonor.X = (float)compare.Red();
                PotentialDonor.Y = (float)compare.Green();
                PotentialDonor.Z = (float)compare.Blue();

                double newDistance = Vector3.Distance(PotentialDonor, PotentialRecipient);

                if(newDistance < Distance && EnsureSingle(initial, compare))
                {
                    Distance = newDistance;
                    chosen = i;
                }


            }
            compare = Occurence_Table![chosen].Value!;

            NewSwap(compare,initial);
            

        }

        void ProximityCompression()
        {
            int chosen_a = 0;
            int chosen_b = 0;

            Pixel15 initial = Occurence_Table! [0].Value!;
            Pixel15 compare = Occurence_Table![0].Value!;

            Vector3 PotentialRecipient = new Vector3();
            Vector3 PotentialDonor = new Vector3();

            double Distance = double.PositiveInfinity;
            double newDistance = Distance;

            for (int i = 0; i <= (Input!.ProtectedBufferSize); i++)
            {

                if (i > 0)
                {
                    Protect(Alpha15!);
                }

                for (int j = (int)(MaxUniqueCount - UniqueCount); j < (int)MaxUniqueCount; j++)
                {

                    initial = Occurence_Table[j].Value!;

                    PotentialRecipient.X = (float)initial.Red();
                    PotentialRecipient.Y = (float)initial.Green();
                    PotentialRecipient.Z = (float)initial.Blue();


                    for (int k = (int)(MaxUniqueCount - UniqueCount); k < (int)MaxUniqueCount; k++)
                    {
                        compare = Occurence_Table[k].Value!;


                        PotentialDonor.X = (float)compare.Red();
                        PotentialDonor.Y = (float)compare.Green();
                        PotentialDonor.Z = (float)compare.Blue();

                        newDistance = Vector3.Distance(PotentialRecipient, PotentialDonor);

                        if (i < (Input.ProtectedBufferSize-1))
                        {
                            if(newDistance<Distance && EnsureDual(initial, compare))
                            {
                                Distance = newDistance;
                                chosen_a = j;
                                chosen_b = k;

                            }

                        }

                        else
                        {
                            if (newDistance < Distance && EnsureSingle(initial, compare))
                            {
                                Distance = newDistance;
                                chosen_a = j;
                                chosen_b = k;

                            }


                        }
                    }

                }


                initial = Occurence_Table![chosen_a].Value!;
                compare = Occurence_Table![chosen_b].Value!;


                if (EnsureDual(initial, compare))
                {

                    break;
                }

            }

            initial = Occurence_Table![chosen_a].Value!;
            compare = Occurence_Table![chosen_b].Value!;

            if(EnsureDual(initial, compare)) {

                Vector3 Saturation = GetHue(initial);

                PotentialRecipient.X = (float)initial.Red();
                PotentialRecipient.Y = (float)initial.Green();
                PotentialRecipient.Z = (float)initial.Blue();

                PotentialDonor.X = (float)compare.Red();
                PotentialDonor.Y = (float)compare.Green();
                PotentialDonor.Z = (float)compare.Blue();

                Distance = Vector3.Distance(Saturation,PotentialRecipient);
                newDistance = Vector3.Distance(Saturation, PotentialRecipient);


                if (Distance<newDistance)
                {
                    Occurence_Table[chosen_a].Occurence += Occurence_Table[chosen_b].Occurence;
                    Occurence_Table[chosen_b].Occurence = 0;
                    NewSwap(initial, compare);
                    Protect(initial);
                }
                else
                {
                    Occurence_Table[chosen_b].Occurence += Occurence_Table[chosen_a].Occurence;
                    Occurence_Table[chosen_a].Occurence = 0;
                    NewSwap(compare, initial);
                    Protect(compare);

                }

            }

            else if (EnsureSingle(initial, compare))
            {

                if (Occurence_Table[chosen_a].Occurence > Occurence_Table[chosen_b].Occurence)
                {
                    Occurence_Table[chosen_a].Occurence += Occurence_Table[chosen_b].Occurence;
                    Occurence_Table[chosen_b].Occurence = 0;
                    NewSwap(initial, compare);
                    Protect(initial);
                }
                else
                {
                    Occurence_Table[chosen_b].Occurence += Occurence_Table[chosen_a].Occurence;
                    Occurence_Table[chosen_a].Occurence = 0;                 
                    NewSwap(compare,initial);
                    Protect(compare);
                    
                }

            }

        }

        byte Get_Shift()
        {
            if (this.Output!.PLT!.Data!.Count > 16)
            {
                return (byte)0;
            }           
            else
            {
                return (byte)4;
            }

        }

        public byte Get_Index(Pixel15 Value)
        {
            Pixel15? Search = Alpha15;

            

            foreach (Swap_Entry sw in this.Swap_Table!)
            {
                if (Value.Equals(sw.Donor!))
                {
                    
                    Search = Value; break;

                }

                else if (Value.Equals(sw.Recipient!))
                {
                    
                    Search = sw.Donor!; break;
                }
               

            }


            return (byte)this.Output!.PLT!.Data!.FindIndex(pxl=>pxl.Equals(Search!));
        }

        public IMG_DATA(ImageDTO input)
        {


            this.Input = input;

        }

        public override async Task Convert(DarkforgeDBContext ctx) { 


            this.Image = await ctx.BMPs.Where(b => b.Hash == this.Input!.ImageHash).FirstAsync();

           

            if (this.Input!.Alpha != null)
            {
                this.Alpha = new Pixel24((byte)this.Input.Alpha[0], (byte)this.Input.Alpha[1], (byte)this.Input.Alpha[2]);

                this.Alpha15 = new Pixel15(this.Alpha,false);               
               
            }

           

            this.Occurence_Table = new List<Occurence_Entry>();
            this.Swap_Table = new List<Swap_Entry>();

            foreach (Pixel24 p in this.Image.Data!)
            {
                bool new_clr = true;

                Pixel15 val = new Pixel15(p,!p.Equals(this.Alpha!));

                foreach (Occurence_Entry oe in this.Occurence_Table)
                {
                    if(val.Data==oe.Value!.Data)
                    {
                        new_clr = false;
                    }
                }

                if (new_clr)
                {
                    NewColour(val);
                }

                else
                {
                    Occurence_Entry occ = this.Occurence_Table.Where(o=>o.Value!.Data==val!.Data).First();
                    occ.Occurence++;
                }
            }

            this.Occurence_Table=this.Occurence_Table!.OrderBy(o=>o.Occurence).ToList();

            this.MaxUniqueCount = (uint)this.Occurence_Table.Count();
            this.UniqueCount = this.MaxUniqueCount;


            if(this.Input.ProtectedBufferSize > 0)
            {
                this.ProtectedBuffer = new List<Pixel15>();
                for (int i = 0; i < Input.ProtectedBufferSize; i++)
                {
                    this.ProtectedBuffer.Add(new Pixel15());
                }
                this.ProtectedBufferIndex = 0;   
            }

            
            if (this.UniqueCount > ((uint)this.Input.CLUT_Size + 1))
            {

                while (this.UniqueCount > ((uint)this.Input.CLUT_Size + 1))
                {

                    for (int i = 0; i < this.MaxUniqueCount; i++)
                    {

                        if (this.Input.Mode)
                        {
                            this.ProximityCompression();
                        }

                        else
                        {
                            this.PopularityCompression(i);
                        }


                        if (this.UniqueCount <= ((uint)this.Input.CLUT_Size + 1))
                        {

                            foreach (Occurence_Entry oe in this.Occurence_Table)
                            {
                                Swap_Entry new_swap = new Swap_Entry();
                                new_swap.Recipient = oe.Value;
                                new_swap.Donor = oe.Value;

                                this.Swap_Table.Add(new_swap);
                            }

                            break;
                        }

                    }
                                       


                }

            }

            else
            {
                foreach (Occurence_Entry oe in this.Occurence_Table)
                {
                    Swap_Entry new_swap = new Swap_Entry();
                    new_swap.Recipient = oe.Value;
                    new_swap.Donor = oe.Value;

                    this.Swap_Table.Add(new_swap);
                }

            }

            this.Output = new RPF();
            this.Output.PLT = new PLT();
            this.Output.PGA = new PGA();

            this.Output.PLT.Data = this.Occurence_Table.Where(ot => ot.Occurence > 0).Select(ot => ot.Value!).ToList();

            if (Alpha15 != null)
            {

                for (int i = 0; i < this.Output.PLT.Data.Count; i++)
                {
                    if (this.Output.PLT.Data[i].Equals(Alpha15!))
                    {
                        this.Output.PLT.Data[i].Data = 0x0000;
                        break;
                    }                  

                }

            }


            this.Shift_Value = Get_Shift();

            this.Output.Serialize(this);

            this.Output.Width=(byte)(this.Image.Width-1);
            this.Output.Height = (byte)(this.Image.Height - 1);

            this.Output.CLUT = (byte)(this.Output.PLT.Data.Count-1);


            this.AlphaUsed = false;
            if (this.Alpha15 != null)
            {
                if (this.Occurence_Table.Where(o => o.Value?.Equals(this.Alpha15) == true).Count() > 0)
                {
                    this.AlphaUsed = true;
                }
            }
            



            await ctx.PLTs.AddAsync(this.Output.PLT);
            await ctx.PGAs.AddAsync(this.Output.PGA);
            await ctx.SaveChangesAsync();

            this.Output.PLT_ID = this.Output.PLT.ID;  
            this.Output.PGA_ID = this.Output.PGA.ID;


            await ctx.RPFs.AddAsync(this.Output);
            await ctx.SaveChangesAsync();

            this.Occurence_Table?.Clear();
            this.Swap_Table?.Clear();
            this.ProtectedBuffer?.Clear();


           

        }

        
    }

}

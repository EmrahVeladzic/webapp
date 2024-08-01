using backend.Models;
using backend.Requests;
using System.Numerics;
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


    public class IMG_DATA
    {
        private Pixel24? Alpha { get; set; }
        private Pixel15? Alpha15 { get; set; }

        private UInt32 ScaleX { get; set; }
        private UInt32 ScaleY { get; set; }

        private ImageJson? Input { get; set; }

        private UInt32 ProtectedBufferIndex {get;set;}

        private List<Pixel15>? ProtectedBuffer { get; set; }

        private List<Occurence_Entry>? Occurence_Table { get; set; }

        private List<Swap_Entry>? Swap_Table { get; set;}

        private RPF? Output { get; set; }

        private UInt32 UniqueCount { get; set; }
        private UInt32 MaxUniqueCount { get; set; }


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
                    if (a.Data == ProtectedBuffer![i].Data || b.Data == ProtectedBuffer![i].Data)
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


            List<Swap_Entry> toUpdate = Swap_Table!.Where(s => s.Donor!.Data == recipient.Data).ToList();

            for (int i = 0; i < toUpdate.Count(); i++)
            {
                toUpdate[i].Donor = donor;
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

            for (int i = 0; i < (Input!.ProtectedBufferSize+1); i++)
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

                        if (i < Input.ProtectedBufferSize)
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

    }

}

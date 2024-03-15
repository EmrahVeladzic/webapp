using backend.Models;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace backend.Converters
{

    public class IMG_DATA
    {

        public List<Pixel15> BMP_CONVERT { get; set; }

        public IMG_DATA(BMP input, List<UInt16> user_defined)
        {
            
            BMP_CONVERT= new List<Pixel15>();

            Pixel15 TMP = new Pixel15();

            foreach (Pixel24 pixel in input.Data!)
            {                               

                TMP.Setup(pixel);

                foreach (UInt16 pixel2 in user_defined)
                {

                    TMP.Assert_User_Defined(pixel2);

                }


                BMP_CONVERT.Add(TMP);

            }

            
        }


      

    }


    public class Occurence_
    {
        public Pixel15 Colour { get; set; }

        public int Occurence { get; set; }

        //Predefined colour by user. Overrides colours close to it. Does not get overridden.
       

        //Final Colours which will be written to CLUT.
        public bool Dominant { get; set; }


        //Alpha - if false, represents transparency.
        //No interpolation - The value after conversion to 15-bit must be exactly as user-defined to become transparent.
        public bool Alpha { get; set; }



        //Once a colour becomes a recipient, it is no longer live, meaning it will no longer appear in the final RPF unless rearmed.
        public bool Live { get; set; }


        //Used for calculating vector distance between 2 colours. 
        public Vector3 Vector_Clr { get; set; }

        public void Increment()
        {
            this.Occurence++;
        }

        public void Reset()
        {
            this.Occurence = 0;
        }


        public void Disarm()
        {
            this.Live = false;
        }

        public Occurence_(Pixel15 clr, bool dominance)
        {
            this.Colour = clr;
            this.Dominant = dominance;
            this.Alpha = (clr.Alpha()==1)? true:false;
            this.Occurence = 1;
            this.Vector_Clr = new Vector3(clr.Blue(),clr.Green(),clr.Red());
            this.Live = true;

        }



    }

    public class Swap_
    {
        //This list stores the decision on which colour becomes which. 

        public Pixel15 Donor { get; set; }

        public Pixel15 Recipient { get; set; }


        public Swap_(Pixel15 dnr, Pixel15 rcp)
        {
            this.Donor = dnr;
            this.Recipient = rcp;
        }

        public void SetDonor(Pixel15 dnr)
        {
            this.Donor = dnr;
        }
    }


    public class Chunk_
    {
        public int Begin_X { get; set; }

        public int Begin_Y { get; set; }       

        public int Order { get; set; }

        public int MaxClr { get; set; }

        public int ReservedClr { get; set; }


        public Chunk_(int begX, int begY, int ord, int max)
        {
            this.Begin_X = begX;
            this.Begin_Y = begY;
            this.Order = ord;
            this.MaxClr = max;
            this.ReservedClr = 0;
        }

        public void Carry(int add)
        {
            this.MaxClr += add;
        }

        public void NewReserved()
        {
            this.ReservedClr++;
        }

    }



    public class BMP_RPF_Converter
    {

        public IMG_DATA RawData {  get; set; }

        public List<Occurence_>? Occurence_Table {  get; set; } 

        public List<Swap_>? Swap_Table { get; set; }

        public List<Chunk_>? ImageChunks { get; set; }


        public int ChunkWidth { get; set; }

        public int ChunkHeight { get; set; }



        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }



        public BMP_RPF_Converter(BMP input , byte unique_texel, List<UInt16> reserved_clut, byte X_part, byte Y_part, List<byte> chunk_max, List<byte> pecking_order)
        {

            this.ImageHeight = input.Height;
            this.ImageWidth = input.Width;

            this.ChunkWidth = input.Width/(int)X_part;
            this.ChunkHeight = input.Height/(int)Y_part;

            this.Occurence_Table = new List<Occurence_>();
            this.Swap_Table = new List<Swap_>();
            this.ImageChunks = new List<Chunk_>();


            PopulateChunkTable((int)Y_part,(int)X_part,chunk_max,pecking_order);

            LoadUserDefinedAsDominant(reserved_clut);           

            this.RawData= new IMG_DATA(input, reserved_clut);


            PopulateOccurrenceTable();
          

            Occurence_Table = Occurence_Table.OrderByDescending(o=>o.Occurence).ToList();

            
           BeginCompression(chunk_max, pecking_order);
            
            
        }


        private void BeginCompression(List<byte> chunk_max, List<byte> pecking_order)
        {
            int max_contrib_carry = 0;

            foreach (byte ord in pecking_order)
            {
                
                List<Occurence_>local_occurence_table = new List<Occurence_>();

                this.ImageChunks![(int)ord].Carry(max_contrib_carry);
                max_contrib_carry = 0;

                for (int y = this.ImageChunks[(int)ord].Begin_Y; y < this.ImageChunks[(int)ord].Begin_Y + this.ChunkHeight; y++)
                {

                    for (int x = this.ImageChunks[(int)ord].Begin_X; x < this.ImageChunks[(int)ord].Begin_X + this.ChunkWidth; x++)
                    {

                        bool new_clr = true;

                        foreach (Occurence_ chk_clr in local_occurence_table)
                        {
                            if (this.RawData.BMP_CONVERT[(y * this.ImageWidth) + x] == chk_clr.Colour)
                            {
                                new_clr = false;
                                chk_clr.Increment();
                            }
                        }

                        if (new_clr)
                        {
                            
                            Occurence_? local_occurrence = this.Occurence_Table!.Where(ot => ot.Colour == this.RawData.BMP_CONVERT[(y * this.ImageWidth) + x]).FirstOrDefault();
                            local_occurrence!.Reset();
                            local_occurence_table.Add(local_occurrence!);
                        }


                    }



                }


                List<Pixel15> toRemove = new List<Pixel15>();

                foreach (Occurence_ oc in local_occurence_table)
                {
                   if(oc.Dominant==true || oc.Alpha==true || oc.Live == false)
                    {
                        toRemove.Add(oc.Colour);
                    }

                   
                }

                foreach (Pixel15 px in toRemove)
                {
                    Occurence_? rmv = local_occurence_table!.Where(o => o.Colour == px).FirstOrDefault();

                    local_occurence_table.Remove(rmv!);
                }

                toRemove.Clear();

                local_occurence_table = local_occurence_table.OrderBy(o => o.Occurence).ToList();

                while (local_occurence_table.Count() > chunk_max[(int)ord])
                {
                    Find_Closest_Via_Vector(local_occurence_table[0]);
                    local_occurence_table.RemoveAt(0);
                 
                }

                foreach (Occurence_ ocr in local_occurence_table)
                {
                    Occurence_? globl = this.Occurence_Table!.Where(o=>o.Colour == ocr.Colour).FirstOrDefault();

                    globl!.Dominant = true;

                }


                max_contrib_carry = this.ImageChunks[(int)ord].MaxClr - this.ImageChunks[(int)ord].ReservedClr;
                local_occurence_table.Clear();

            }

            this.Occurence_Table=this.Occurence_Table!.OrderByDescending(o=>o.Occurence).ToList();


            for (int i = 0; i < max_contrib_carry; i++)
            {
                foreach (Occurence_ ocr in this.Occurence_Table!)
                {
                    if (ocr.Live==false)
                    {
                        Revive(ocr);

                        break;
                    }
                }
            }


            EditRawData();
            Write_To_RPF();
        }

        private void EditRawData()
        {
            foreach (Pixel15 px in this.RawData.BMP_CONVERT)
            {
               foreach(Swap_ swp in this.Swap_Table!)
                {
                    if (px == swp.Recipient)
                    {
                        px.Swap(swp.Donor);
                    }
                }
            }
        }

        private void Write_To_RPF()
        {
            PLT CLUT = new PLT();

            CLUT.Data = this.Occurence_Table!.Where(o => o.Dominant == true).Select(o => o.Colour).ToList();

            PGA PXGrid = new PGA();

            byte tmp = 0;

            for (int i = 0; i < this.RawData.BMP_CONVERT.Count; i++)
            {
                for (int j = 0; j < CLUT.Data.Count; j++)
                {
                    if (RawData.BMP_CONVERT[i] == CLUT.Data[j])
                    {
                        tmp = (byte)j;
                        PXGrid.Data!.Add(tmp);
                    }
                }
            }
            
            


        }

        private void Revive(Occurence_ input)
        {
            input.Live = true;

            Swap_? swp = Swap_Table!.Where(s=>s.Recipient==input.Colour).FirstOrDefault();

            swp!.Donor=swp.Recipient;

            input.Dominant=true;

        }

        private void PopulateChunkTable(int Y_part, int X_part , List<byte> chunk_max, List<byte> pecking_order)
        {
            for (int y = 0; y < Y_part; y++)
            {

                for (int x = 0; x < X_part; x++)
                {
                    Chunk_ chnk = new Chunk_((x * (ImageWidth / ChunkWidth)), (y * (ImageHeight / ChunkHeight)), pecking_order[(y * (ImageWidth / ChunkWidth)) + x], chunk_max[(y * (ImageWidth / ChunkWidth)) + x]);

                    this.ImageChunks!.Add(chnk);
                }

            }
        }

        private void PopulateOccurrenceTable()
        {
            foreach (Pixel15 pxl in this.RawData.BMP_CONVERT)
            {
                bool new_clr = true;

                foreach (Occurence_ txl in this.Occurence_Table!)
                {
                    if (pxl == txl.Colour)
                    {
                        txl.Increment();
                        new_clr = false;
                        break;
                    }
                }

                if (new_clr)
                {
                    Occurence_ tmp = new Occurence_(pxl, true);

                    this.Occurence_Table.Add(tmp);

                    this.Occurence_Table.Add(tmp);
                }



            }

        }


        private void LoadUserDefinedAsDominant(List<UInt16> reserved_clut)
        {
            foreach (UInt16 clr in reserved_clut)
            {
                Pixel15 px = new Pixel15(clr);

                Occurence_ tmp = new Occurence_(px, true);

                Occurence_Table!.Add(tmp);
            }

        }

        private void Find_Closest_Via_Vector(Occurence_ input)
        {
            Pixel15 closest = input.Colour;

            float distance = float.MaxValue;

            foreach (Occurence_ ocr in this.Occurence_Table!)
            {
                float new_dist = Vector3.Distance(input.Vector_Clr, ocr.Vector_Clr);

                if (new_dist<=distance && input.Colour != ocr.Colour && ocr.Alpha==true)
                {
                    closest = ocr.Colour;
                    distance= new_dist;
                }
            }

            Occurence_? donor = this.Occurence_Table!.Where(o => o.Colour == closest).FirstOrDefault();

            CreateSwap(donor!, input);
        }


        private void CreateSwap(Occurence_ Donor, Occurence_ Recipient)
        {

            Occurence_? globl = this.Occurence_Table!.Where(o => o.Colour == Recipient.Colour).FirstOrDefault();
            globl!.Live = false;

            Swap_ swp = new Swap_(Donor.Colour,Recipient.Colour);
            this.Swap_Table!.Add(swp);
            CascadeSwap(Donor.Colour, Recipient.Colour);
            
        }

        private void CascadeSwap(Pixel15 Donor, Pixel15 Recipient)
        {
            foreach(Swap_ sw in this.Swap_Table!)
            {
                if (sw.Donor == Recipient)
                {
                    sw.SetDonor(Donor);
                }
            }
        }

        
    }
}

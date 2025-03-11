using backend.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("AST",Schema ="Models")]
    public class AST:BaseEntity, ITopLevelModel
    {
        [JsonIgnore]
        [Column("MDLID")]
        public int? MDL_ID { get; set; }

        [JsonIgnore]
        [Column("FKRID")]
        public int? FKR_ID { get; set; }

        
        [ForeignKey(nameof(MDL_ID))]
        public virtual MDL? MDL {  get; set; }

        [ForeignKey(nameof(FKR_ID))]
        public virtual FKR? FKR { get; set; }

        [Column("PrecisionBits")]
        public byte PrecisionBits { get; set; }


        public AST():base()
        {
            this.MDL = null;
            this.FKR = null;

            this.MDL_ID = null;
            this.FKR_ID = null;
        }

        public override void Serialize()
        {

            this.MDL?.Serialize();
            this.FKR?.Serialize();
        }

        public override void Deserialize()
        {
            this.MDL?.Deserialize();
            this.FKR?.Deserialize();


            if (this.FKR_ID != null)
            {
                this.FKR!.Root = this.FKR.Bones!.Where(b => b.Parent_ID == null).Select(b => b.ID).FirstOrDefault();
            }


        }

        public override void Clear()
        {
            this.MDL?.Clear();
            this.FKR?.Clear();

            this.MDL = null;
            this.FKR = null;
        }

        public byte[] ToArrayBuffer()
        {
            List<byte> temp = new List<byte>();

            temp.Add(65);
            byte[]? TRS = null;
            byte t_byte = 0;
            if (this.MDL != null && this.MDL.Meshes!=null)
            {
                t_byte |= 1 << 7;

                if (this.MDL!.Meshes[0].VT != null)
                {
                    t_byte |= 1 << 6;
                }
                if (this.MDL!.Meshes[0].IND != null)
                {
                    t_byte |= 1 << 5;
                }
                if (this.MDL!.Meshes[0].UV != null)
                {
                    t_byte |= 1 << 4;
                }
                if (this.MDL!.Meshes[0].NRM != null)
                {
                    t_byte |= 1 << 3;
                }
            }
            if (this.FKR != null)
            {
                t_byte |= 1 << 2;

                if (this.FKR.Animations != null)
                {
                    t_byte += 1 << 1;
                }
            }
            temp.Add(t_byte);

            temp.Add(this.PrecisionBits);
            temp.Add(0);
            temp.Add(0);      
            t_byte = 0;            
            if(this.MDL!=null)
            {
                t_byte=this.MDL.Width??0;
            }
            temp.Add(t_byte);       
            if (this.MDL != null)
            {
                t_byte = this.MDL.Height ?? 0;
            }
            temp.Add(t_byte);        

            t_byte = 0;
            if (this.MDL != null)
            {
                t_byte = (byte)this.MDL.Meshes!.Count();
            }
            temp.Add(t_byte);          

            if (this.MDL != null)
            {
                foreach (MSH m in this.MDL!.Meshes!)
                {
                    t_byte = 255;

                    if (m.BN_ID != null && this.FKR!=null)
                    {
                        t_byte=(byte)FKR!.Bones.FindIndex(b=>b.ID==m.BN_ID);
                    }

                    temp.Add(t_byte);

                    if (m.VT != null)
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)m.VT.Vertices.Count, temp);

                        temp.AddRange(m.VT.Serialized!);
                    }
                    else
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)0, temp);
                    }

                    if (m.IND != null)
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)m.IND.Indices.Count, temp);

                        temp.AddRange(m.IND.Serialized!);
                    }
                    else
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)0, temp);
                    }

                    if (m.UV != null)
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)m.UV.TextureCoordinates!.Count, temp);

                        temp.AddRange(m.UV.Serialized!);
                    }
                    else
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)0, temp);
                    }

                    if (m.NRM != null)
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)m.NRM.Normals.Count, temp);

                        temp.AddRange(m.NRM.Serialized!);
                    }
                    else
                    {
                        PrimitiveSerialization.SerializePrimitive((UInt16)0, temp);
                    }

                }
            }

            t_byte = 0;

            if (this.FKR != null)
            {
                t_byte = (byte)this.FKR.Bones.Count;

            }

            temp.Add(t_byte);

            t_byte = 255;

            if (this.FKR != null && this.FKR.Root!=null)
            {
                t_byte =(byte)this.FKR.Bones.FindIndex(b=>b.ID==this.FKR.Root);
            }

            temp.Add(t_byte);

            t_byte = 0;

            if (this.FKR != null && this.FKR.Animations != null)
            {
                t_byte=(byte)this.FKR.Animations.Count;
            }

            temp.Add(t_byte );

            t_byte = 0;
            if (this.FKR != null)
            {
                t_byte = (byte)(this.FKR.FPS!);
            }
            temp.Add(t_byte);

            if (this.FKR != null)
            {

                foreach (BN bn in this.FKR!.Bones)
                {
                    temp.AddRange(bn.Serialized!);

                    t_byte = (byte)(this.FKR.Bones.Where(b => b.Parent_ID == bn.ID).Count());

                    temp.Add(t_byte);

                    for (int i = 0; i < this.FKR!.Bones.Count; i++)
                    {
                        if (this.FKR.Bones[i].Parent_ID == bn.ID)
                        {
                            temp.Add((byte)i);
                        }

                    }

                    if (this.FKR!.Animations != null)
                    {

                        foreach (ANM a in this.FKR.Animations)
                        {
                            foreach (TK t in a.Tracks.Where(t => t.BN_ID == bn.ID))
                            {
                                temp.Add(t.T_Count);


                                TRS = new byte[((int)t.T_Count * 13)];

                                Array.Copy(t.Serialized!, 0, TRS, 0, ((int)t.T_Count * 13));

                                temp.AddRange(TRS);

                                TRS = null;

                                temp.Add(t.R_Count);

                                TRS = new byte[((int)t.R_Count * 17)];

                                Array.Copy(t.Serialized!, ((int)t.T_Count * 13), TRS, 0, ((int)t.R_Count * 17));

                                temp.AddRange(TRS);

                                TRS = null;

                                temp.Add(t.S_Count);

                                TRS = new byte[((int)t.S_Count * 13)];

                                Array.Copy(t.Serialized!, (((int)t.T_Count * 13) + ((int)t.R_Count * 17)), TRS, 0, ((int)t.S_Count * 13));

                                temp.AddRange(TRS);

                                TRS = null;

                            }
                        }

                    }
                }
            }

            byte[] data = temp.ToArray();
            temp.Clear();
            return data;
        }
    }
}

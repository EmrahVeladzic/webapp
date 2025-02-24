using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text.Json;

namespace backend.Converters
{
    public class AST_DATA
    {
        private ModelJson Input { get; set; }

        public AST? Output { get; set; }

        public GLB Model { get; set; }

        public AST_DATA(ModelJson input)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            this.Input = input;

            this.Model = ctx.GLBs.Where(g=>g.Hash==input.ModelHash).First();

            this.Model.Setup(this.Model!.Serialized!,this.Model!.Hash!);

            this.Output = new AST();

            this.Output.PrecisionBits=this.Input.PrecisionBits;

            
            if (Model!.Metadata!.RootElement.TryGetProperty("nodes", out JsonElement nodes) && nodes.ValueKind == JsonValueKind.Array && this.Model.Metadata!.RootElement.TryGetProperty("accessors", out JsonElement accessors)&& accessors.ValueKind==JsonValueKind.Array && this.Model!.Metadata.RootElement.TryGetProperty("bufferViews",out JsonElement buffers)&&buffers.ValueKind==JsonValueKind.Array)
            {


                if (Model!.Metadata!.RootElement.TryGetProperty("skins", out JsonElement skins) && skins.ValueKind == JsonValueKind.Array)
                {

                    if (skins[0].TryGetProperty("joints", out JsonElement joints) && joints.ValueKind == JsonValueKind.Array)
                    {
                        this.Output.FKR = new FKR();

                        ctx.FKRs.Add(this.Output.FKR);

                        ctx.SaveChanges();

                        this.Output.FKR_ID = this.Output.FKR.ID;

                        Int32[] joint_index_array = joints.Deserialize<Int32[]>()!;

                        List<BN> bones = new List<BN>();

                        for (int i = 0; i < joint_index_array.Length; i++)
                        {
                            BN temp_bone = new BN();

                            temp_bone.FKR_ID = this.Output.FKR.ID;

                            temp_bone.Serialize();

                            ctx.BNs.Add(temp_bone);

                            ctx.SaveChanges();

                            bones.Add(temp_bone);

                        }

                        for (int i = 0; i < joint_index_array.Length; i++)
                        {
                            Vector3 translation = Vector3.Zero;
                            Quaternion rotation = Quaternion.Identity;
                            Vector3 scale = Vector3.One;


                            if (nodes[joint_index_array[i]].TryGetProperty("translation", out JsonElement trans) && trans.ValueKind == JsonValueKind.Array)
                            {
                                float[] t = trans.Deserialize<float[]>()!;

                                translation = new Vector3(t[0], t[1], t[2]);
                            }

                            if (nodes[joint_index_array[i]].TryGetProperty("rotation", out JsonElement rot) && rot.ValueKind == JsonValueKind.Array)
                            {
                                float[] r = rot.Deserialize<float[]>()!;

                                rotation = new Quaternion(r[0], r[1], r[2], r[3]);
                            }


                            if (nodes[joint_index_array[i]].TryGetProperty("scale", out JsonElement scal) && scal.ValueKind == JsonValueKind.Array)
                            {
                                float[] s = scal.Deserialize<float[]>()!;

                                scale = new Vector3(s[0], s[1], s[2]);
                            }

                            bones[i].InitialTransform = new FTransform(new Transform(translation, rotation, scale));
                            


                            if (nodes[i].TryGetProperty("children", out JsonElement child_joints) && child_joints.ValueKind == JsonValueKind.Array)
                            {
                                int[] children = child_joints.Deserialize<int[]>()!;

                                for (int j = 0; j < children.Length; j++)
                                {
                                    bones[j].Parent_ID = bones[i].ID;
                                }

                            }

                            ctx.SaveChanges();

                        }

                    }


                }


                if (Model!.Metadata.RootElement.TryGetProperty("meshes", out JsonElement submeshes) && submeshes.ValueKind == JsonValueKind.Array)
                {

                    this.Output.MDL = new MDL();
                    ctx.MDLs.Add(this.Output.MDL);

                    ctx.SaveChanges();

                    this.Output.MDL_ID = this.Output.MDL.ID;

                    List<MSH> meshes = new List<MSH>();

                    for (int i = 0; i < submeshes.GetArrayLength(); i++)
                    {
                        MSH mesh = new MSH();

                        mesh.MDL_ID = this.Output.MDL.ID;                        

                        ctx.MSHs.Add(mesh);

                        ctx.SaveChanges();

                        meshes.Add(mesh);

                    }

                    for (int i = 0; i < submeshes.GetArrayLength(); i++)
                    {

                        if (submeshes[i].TryGetProperty("primitives",out JsonElement primitives)&& primitives.ValueKind==JsonValueKind.Array)
                        {
                            if (primitives[0].TryGetProperty("indices",out JsonElement a_indices)&& a_indices.TryGetInt32(out Int32 access))
                            {

                                if (accessors[access].TryGetProperty("bufferView",out JsonElement b_indices)&&b_indices.TryGetInt32(out Int32 view))
                                {

                                    if (buffers[view].TryGetProperty("byteLength",out JsonElement b_len)&& b_len.TryGetInt32(out Int32 length) && buffers[view].TryGetProperty("byteOffset",out JsonElement b_off)&&b_off.TryGetInt32(out Int32 offset))
                                    {
                                        

                                        meshes[i].IND = new IND();
                                        ctx.INDs.Add(meshes[i].IND!);

                                        ctx.SaveChanges() ;

                                        meshes[i].IND_ID = meshes[i].IND!.ID;

                                        for (int j = offset; j < (offset+length); j+=sizeof(UInt16))
                                        {

                                           UInt16 temp = BitConverter.ToUInt16(this.Model!.BLOB!,j);

                                           meshes[i].IND!.Indices.Add(temp);

                                        }



                                    }


                                }                            

                            }

                          

                        }


                    }

                }


            }

           


            ctx.SaveChanges();

            ctx.Dispose();

        }



    }
}

using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Json;

namespace backend.Converters
{
    public class AST_DATA
    {
        private ModelJson Input { get; set; }

        public AST? Output { get; set; }

        public GLB Model { get; set; }

        public List<BN>? Bones { get; set; }

        public List<MSH>? Meshes { get; set; }
        public Int32[]? Joint_Index_Array { get; set; }

        public AssetJson? Asset { get; set; }

        public byte GetFrame(byte FPS, float time)
        {
            return (byte)((UInt64)(Math.Round(((float)(FPS-1)*time)))%(UInt64)FPS);
        }

        public AST_DATA(ModelJson input)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            this.Input = input;

            this.Model = ctx.GLBs.Where(g=>g.Hash==input.ModelHash).First();

            this.Model.Setup(this.Model!.Serialized!,this.Model!.Hash!);

            this.Output = new AST();

            ctx.ASTs.Add(this.Output);

            this.Output.PrecisionBits=this.Input.PrecisionBits;


            
            if (Model!.Metadata!.RootElement.TryGetProperty("nodes", out JsonElement nodes) && nodes.ValueKind == JsonValueKind.Array && this.Model.Metadata!.RootElement.TryGetProperty("accessors", out JsonElement accessors)&& accessors.ValueKind==JsonValueKind.Array && this.Model!.Metadata.RootElement.TryGetProperty("bufferViews",out JsonElement buffers)&&buffers.ValueKind==JsonValueKind.Array)
            {
                
                

                if (Model!.Metadata!.RootElement.TryGetProperty("skins", out JsonElement skins) && skins.ValueKind == JsonValueKind.Array)
                {

                    if (skins[0].TryGetProperty("joints", out JsonElement joints) && joints.ValueKind == JsonValueKind.Array)
                    {
                        Bones = new List<BN>();

                        this.Output.FKR = new FKR();

                        ctx.FKRs.Add(this.Output.FKR);

                        ctx.SaveChanges();

                        this.Output.FKR_ID = this.Output.FKR.ID;

                        Joint_Index_Array = joints.Deserialize<Int32[]>()!;

                       

                        for (int i = 0; i < Joint_Index_Array.Length; i++)
                        {
                            BN temp_bone = new BN();

                            temp_bone.FKR_ID = this.Output.FKR.ID;

                           
                            ctx.BNs.Add(temp_bone);

                            ctx.SaveChanges();

                            Bones.Add(temp_bone);

                            this.Output.FKR.Bones.Add(temp_bone);

                        }

                        for (int i = 0; i < Joint_Index_Array.Length; i++)
                        {
                            Vector3 translation = Vector3.Zero;
                            Quaternion rotation = Quaternion.Identity;
                            Vector3 scale = Vector3.One;


                            if (nodes[i].TryGetProperty("translation", out JsonElement trans) && trans.ValueKind == JsonValueKind.Array)
                            {
                                float[] t = trans.Deserialize<float[]>()!;

                                translation = new Vector3(t[0], t[1], t[2]);
                            }

                            if (nodes[i].TryGetProperty("rotation", out JsonElement rot) && rot.ValueKind == JsonValueKind.Array)
                            {
                                float[] r = rot.Deserialize<float[]>()!;

                                rotation = new Quaternion(r[0], r[1], r[2], r[3]);
                            }


                            if (nodes[i].TryGetProperty("scale", out JsonElement scal) && scal.ValueKind == JsonValueKind.Array)
                            {
                                float[] s = scal.Deserialize<float[]>()!;

                                scale = new Vector3(s[0], s[1], s[2]);
                            }

                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(translation.X,this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(translation.Y, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(translation.Z, this.Input.PrecisionBits));
                                  
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(rotation.X, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(rotation.Y, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(rotation.Z, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(rotation.W, this.Input.PrecisionBits));
                                
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(scale.X, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(scale.Y, this.Input.PrecisionBits));
                            Bones[Joint_Index_Array[i]].InitialTransform.Add(FixedPoint.GetFixed<Int32>(scale.Z, this.Input.PrecisionBits));


                            if (nodes[i].TryGetProperty("children", out JsonElement child_joints) && child_joints.ValueKind == JsonValueKind.Array)
                            {
                                int[] children = child_joints.Deserialize<int[]>()!;

                                for (int j = 0; j < children.Length; j++)
                                {
                                    Bones[Joint_Index_Array[children[j]]].Parent_ID = Bones[Joint_Index_Array[i]].ID;
                                   
                                }

                               

                            }



                            ctx.SaveChanges();

                            

                        }

                    }


                }


                if (Model!.Metadata.RootElement.TryGetProperty("meshes", out JsonElement subMeshes) && subMeshes.ValueKind == JsonValueKind.Array)
                {

                    this.Output.MDL = new MDL();
                    ctx.MDLs.Add(this.Output.MDL);

                    ctx.SaveChanges();

                    this.Output.MDL_ID = this.Output.MDL.ID;

                    Meshes = new List<MSH>();

                    for (int i = 0; i < subMeshes.GetArrayLength(); i++)
                    {
                        MSH mesh = new MSH();

                        mesh.MDL_ID = this.Output.MDL.ID;                        

                        ctx.MSHs.Add(mesh);

                        ctx.SaveChanges();

                        Meshes.Add(mesh);

                        this.Output.MDL.Meshes.Add(mesh);

                    }

     

                    for (int i = 0; i < subMeshes.GetArrayLength(); i++)
                    {

                        if (subMeshes[i].TryGetProperty("primitives",out JsonElement primitives)&& primitives.ValueKind==JsonValueKind.Array)
                        {
                            if (primitives[0].TryGetProperty("indices",out JsonElement a_indices)&& a_indices.TryGetInt32(out Int32 ind_access))
                            {

                                if (accessors[ind_access].TryGetProperty("bufferView",out JsonElement b_indices)&&b_indices.TryGetInt32(out Int32 ind_view))
                                {

                                    if (buffers[ind_view].TryGetProperty("byteLength",out JsonElement len)&& len.TryGetInt32(out Int32 length) && buffers[ind_view].TryGetProperty("byteOffset",out JsonElement off)&&off.TryGetInt32(out Int32 offset))
                                    {
                                        

                                        Meshes[i].IND = new IND();
                                        ctx.INDs.Add(Meshes[i].IND!);

                                        ctx.SaveChanges() ;

                                        Meshes[i].IND_ID = Meshes[i].IND!.ID;

                                        for (int j = offset; j < (offset+length); j+=sizeof(UInt16))
                                        {

                                           UInt16 temp = BitConverter.ToUInt16(this.Model!.BLOB!,j);

                                           Meshes[i].IND!.Indices.Add(temp);

                                        }



                                    }


                                }                            

                            }


                            if (primitives[0].TryGetProperty("attributes", out JsonElement attributes)&& attributes.ValueKind==JsonValueKind.Object)
                            {
                              
                                if(attributes.TryGetProperty("POSITION", out JsonElement a_vertices)&&a_vertices.TryGetInt32(out Int32 vert_access)){

                                    if (accessors[vert_access].TryGetProperty("bufferView",out JsonElement b_vertices)&& b_vertices.TryGetInt32(out Int32 vert_view))
                                    {

                                        if (buffers[vert_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[vert_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                        {


                                            Meshes[i].VT = new VT();
                                            ctx.VTs.Add(Meshes[i].VT!);

                                            ctx.SaveChanges();

                                            Meshes[i].VT_ID = Meshes[i].VT!.ID;

                                            for (int j = offset; j < (offset + length); j += (3*sizeof(float)))
                                            {

                                                float x = BitConverter.ToSingle(this.Model!.BLOB!, j);

                                                float y = BitConverter.ToSingle(this.Model!.BLOB!, (j+sizeof(float)));

                                                float z = BitConverter.ToSingle(this.Model!.BLOB!, (j+(2*sizeof(float))));


                                                Meshes[i].VT!.Vertices.Add(FixedPoint.GetFixed<Int32>(x,this.Input.PrecisionBits));
                                                Meshes[i].VT!.Vertices.Add(FixedPoint.GetFixed<Int32>(y, this.Input.PrecisionBits));
                                                Meshes[i].VT!.Vertices.Add(FixedPoint.GetFixed<Int32>(z, this.Input.PrecisionBits));
                                            }



                                        }



                                    }

                                }

                                if (attributes.TryGetProperty("NORMAL", out JsonElement a_normals) && a_normals.TryGetInt32(out Int32 nrm_access))
                                {

                                    if (accessors[nrm_access].TryGetProperty("bufferView", out JsonElement b_normals) && b_normals.TryGetInt32(out Int32 nrm_view))
                                    {

                                        if (buffers[nrm_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[nrm_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                        {


                                            Meshes[i].NRM = new NRM();
                                            ctx.NRMs.Add(Meshes[i].NRM!);

                                            ctx.SaveChanges();

                                            Meshes[i].NRM_ID = Meshes[i].NRM!.ID;

                                            for (int j = offset; j < (offset + length); j += (3 * sizeof(float)))
                                            {

                                                float x = BitConverter.ToSingle(this.Model!.BLOB!, j);

                                                float y = BitConverter.ToSingle(this.Model!.BLOB!, (j + sizeof(float)));

                                                float z = BitConverter.ToSingle(this.Model!.BLOB!, (j + (2 * sizeof(float))));


                                                Meshes[i].NRM!.Normals.Add(FixedPoint.GetFixed<Int32>(x, this.Input.PrecisionBits));
                                                Meshes[i].NRM!.Normals.Add(FixedPoint.GetFixed<Int32>(y, this.Input.PrecisionBits));
                                                Meshes[i].NRM!.Normals.Add(FixedPoint.GetFixed<Int32>(z, this.Input.PrecisionBits));
                                            }



                                        }



                                    }

                                }

                                if (attributes.TryGetProperty("TEXCOORD_0", out JsonElement a_uvs) && a_uvs.TryGetInt32(out Int32 uv_access))
                                {

                                    if (accessors[uv_access].TryGetProperty("bufferView", out JsonElement b_uv) && b_uv.TryGetInt32(out Int32 uv_view))
                                    {

                                        if (buffers[uv_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[uv_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                        {


                                            Meshes[i].UV = new UV();

                                            Meshes[i].UV!.Width = input.TexWidth;
                                            Meshes[i].UV!.Height=input.TexHeight;

                                            ctx.UVs.Add(Meshes[i].UV!);



                                            ctx.SaveChanges();

                                            Meshes[i].UV_ID = Meshes[i].UV!.ID;

                                            for (int j = offset; j < (offset + length); j += (2 * sizeof(float)))
                                            {

                                                float x = BitConverter.ToSingle(this.Model!.BLOB!, j);

                                                float y = BitConverter.ToSingle(this.Model!.BLOB!, (j + sizeof(float)));                                               
                                               

                                                UInt16 w = (UInt16)((Int32)(this.Input.TexWidth)+1);
                                                UInt16 h = (UInt16)((Int32)(this.Input.TexHeight) + 1);


                                                Meshes[i].UV!.TextureCoordinates.Add((byte)((UInt16)(Math.Round(x * (float)w)) % w));
                                                Meshes[i].UV!.TextureCoordinates.Add((byte)((UInt16)(Math.Round(y * (float)h)) % h));


                                            }



                                        }



                                    }

                                }


                                if (attributes.TryGetProperty("JOINTS_0", out JsonElement a_joints) && a_joints.TryGetInt32(out Int32 jnt_access))
                                {

                                    if (accessors[jnt_access].TryGetProperty("bufferView", out JsonElement b_joints) && b_joints.TryGetInt32(out Int32 jnt_view))
                                    {

                                        if (buffers[jnt_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[jnt_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                        {

                                            List<byte> mesh_j = new List<byte>();

                                            for (int j = offset; j < (offset + (4*sizeof(byte))); j += sizeof(byte))
                                            {

                                                mesh_j.Add(Model!.BLOB![j]);
                                              
                                            }

                                            mesh_j.OrderDescending();

                                            if (Bones!=null && Joint_Index_Array!=null)
                                            {
                                                Meshes[i].BN_ID = Bones![Joint_Index_Array[mesh_j[0]]].ID;

                                                
                                            }

                                        }



                                    }

                                }




                            }


                        }


                    }

                }


                if(Model!.Metadata.RootElement.TryGetProperty("animations",out JsonElement anims) && anims.ValueKind == JsonValueKind.Array)
                {

                    for (int i = 0; i < anims.GetArrayLength(); i++) {
                    
                        ANM anim = new ANM();

                        anim.FKR_ID = (int)this.Output.FKR_ID!;

                        ctx.ANMs.Add(anim);

                        ctx.SaveChanges();

                        this.Output.FKR!.Animations.Add(anim);


                        if (anims[i].TryGetProperty("channels", out JsonElement channels) && channels.ValueKind == JsonValueKind.Array && anims[i].TryGetProperty("samplers", out JsonElement samplers) && samplers.ValueKind == JsonValueKind.Array)
                        {

                            for (int j = 0; j < channels.GetArrayLength(); j += 3)
                            {
                                TK track = new TK();
                                track.ANM_ID = anim.ID;

                                if (channels[j].TryGetProperty("target", out JsonElement target) && target.ValueKind==JsonValueKind.Object)
                                {
                                    if (target.TryGetProperty("node", out JsonElement node) && node.TryGetInt32(out Int32 bone_index))
                                    {
                                        track.BN_ID = Bones![bone_index].ID;
                                    }
                                }



                                if (channels[j].TryGetProperty("sampler", out JsonElement t_s)&& t_s.TryGetInt32(out int t_samp))
                                {                                
                                    
                                    if( samplers[t_samp].TryGetProperty("input", out JsonElement input_a) && input_a.TryGetInt32(out int input_access))
                                    {
                                        if (accessors[input_access].TryGetProperty("bufferView", out JsonElement input_b) && input_b.TryGetInt32(out int input_view))
                                        {

                                            if (buffers[input_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[input_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for(int k = offset; k<(offset+length); k+=sizeof(float))
                                                {

                                                    track.T_Frames.Add(GetFrame(this.Input.TargetFPS,BitConverter.ToSingle(this.Model.BLOB!,k)));

                                                }
                                     

                                            }

                                        }

                                    }

                                    if (samplers[t_samp].TryGetProperty("output", out JsonElement output_a) && output_a.TryGetInt32(out int output_access))
                                    {
                                        if (accessors[output_access].TryGetProperty("bufferView", out JsonElement output_b) && output_b.TryGetInt32(out int output_view))
                                        {

                                            if (buffers[output_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[output_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for (int k = offset; k < (offset + length); k += (sizeof(float)*3))
                                                {

                                                    track.Translations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!,k),this.Input.PrecisionBits));


                                                    track.Translations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k+sizeof(float)), this.Input.PrecisionBits));


                                                    track.Translations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + (sizeof(float)*2)), this.Input.PrecisionBits));


                                                }


                                            }

                                        }

                                    }



                                }


                                if (channels[j+1].TryGetProperty("sampler", out JsonElement r_s) && r_s.TryGetInt32(out int r_samp))
                                {

                                    if (samplers[r_samp].TryGetProperty("input", out JsonElement input_a) && input_a.TryGetInt32(out int input_access))
                                    {
                                        if (accessors[input_access].TryGetProperty("bufferView", out JsonElement input_b) && input_b.TryGetInt32(out int input_view))
                                        {

                                            if (buffers[input_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[input_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for (int k = offset; k < (offset + length); k += sizeof(float))
                                                {

                                                    track.R_Frames.Add(GetFrame(this.Input.TargetFPS, BitConverter.ToSingle(this.Model.BLOB!, k)));

                                                }


                                            }

                                        }

                                    }

                                    if (samplers[r_samp].TryGetProperty("output", out JsonElement output_a) && output_a.TryGetInt32(out int output_access))
                                    {
                                        if (accessors[output_access].TryGetProperty("bufferView", out JsonElement output_b) && output_b.TryGetInt32(out int output_view))
                                        {

                                            if (buffers[output_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[output_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for (int k = offset; k < (offset + length); k += (sizeof(float) * 4))
                                                {

                                                    track.Rotations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k), this.Input.PrecisionBits));


                                                    track.Rotations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + sizeof(float)), this.Input.PrecisionBits));


                                                    track.Rotations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + (sizeof(float) * 2)), this.Input.PrecisionBits));

                                                   
                                                    track.Rotations.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + (sizeof(float) * 3)), this.Input.PrecisionBits));
                                                }


                                            }

                                        }

                                    }



                                }



                                if (channels[j+2].TryGetProperty("sampler", out JsonElement s_s) && s_s.TryGetInt32(out int s_samp))
                                {

                                    if (samplers[s_samp].TryGetProperty("input", out JsonElement input_a) && input_a.TryGetInt32(out int input_access))
                                    {
                                        if (accessors[input_access].TryGetProperty("bufferView", out JsonElement input_b) && input_b.TryGetInt32(out int input_view))
                                        {

                                            if (buffers[input_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[input_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for (int k = offset; k < (offset + length); k += sizeof(float))
                                                {

                                                    track.S_Frames.Add(GetFrame(this.Input.TargetFPS, BitConverter.ToSingle(this.Model.BLOB!, k)));

                                                }


                                            }

                                        }

                                    }

                                    if (samplers[s_samp].TryGetProperty("output", out JsonElement output_a) && output_a.TryGetInt32(out int output_access))
                                    {
                                        if (accessors[output_access].TryGetProperty("bufferView", out JsonElement output_b) && output_b.TryGetInt32(out int output_view))
                                        {

                                            if (buffers[output_view].TryGetProperty("byteLength", out JsonElement len) && len.TryGetInt32(out Int32 length) && buffers[output_view].TryGetProperty("byteOffset", out JsonElement off) && off.TryGetInt32(out Int32 offset))
                                            {

                                                for (int k = offset; k < (offset + length); k += (sizeof(float) * 3))
                                                {

                                                    track.Scales.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k), this.Input.PrecisionBits));


                                                    track.Scales.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + sizeof(float)), this.Input.PrecisionBits));


                                                    track.Scales.Add(FixedPoint.GetFixed<Int32>(BitConverter.ToSingle(this.Model.BLOB!, k + (sizeof(float) * 2)), this.Input.PrecisionBits));


                                                }


                                            }

                                        }

                                    }



                                }

                                anim.Tracks.Add(track);

                                ctx.TKs.Add(track);

                                ctx.SaveChanges();




                            }

                        }
                    
                    }

                   
                }
            }

           

            this.Output.Serialize();


            ctx.SaveChanges();

            ctx.Dispose();

            this.Asset = new AssetJson();

            this.Asset.Asset = this.Output;
            this.Asset.AST_ID = this.Output.ID;

            if (this.Output.FKR_ID!=null)
            {
                this.Asset.Root = this.Bones!.Where(b=>b.Parent_ID==null).Select(b=>b.ID).FirstOrDefault();
            }

            this.Asset.PrecisionBits = this.Output.PrecisionBits;

        }



    }
}

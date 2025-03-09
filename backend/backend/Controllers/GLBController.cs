using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Logging;
using backend.Models;
using backend.Requests;
using backend.Users;
using backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GLBController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ModelDTO input)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401);
            }

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                ActiveAST? Optimization = await ctx.ActiveASTs.Where(a => a.FileID == input.ModelHash).Where(a=>a.PrecisionBits==input.PrecisionBits).Where(a=>(a.FPS==input.TargetFPS)||a.FPS==null).Where(a=>(a.Tex_Width==input.TexWidth||a.Tex_Width==null)&&(a.Tex_Height==input.TexHeight||a.Tex_Height==null)).FirstOrDefaultAsync();

                if (Optimization != null)
                {
                    return StatusCode(200, Optimization.Id);
                }



                GLB? temp = await ctx.GLBs.Where(g => g.Hash == input.ModelHash).FirstOrDefaultAsync();

                if (temp == null)
                {
                    if (input.ModelData == null)
                    {

                        
                        return StatusCode(204);

                    }

                    else
                    {
                        StringBuilder stringBuilder = new StringBuilder(input.ModelData!, input.ModelData!.Length);

                        stringBuilder.Replace("\r\n", String.Empty);
                        stringBuilder.Replace(" ", String.Empty);
                        stringBuilder.Replace("data:application/octet-stream;base64,", String.Empty);

                        byte[] Data = System.Convert.FromBase64String(stringBuilder.ToString());

                        temp = new GLB();

                        temp!.Setup(Data, input!.ModelHash!);

                        await ctx.GLBs.AddAsync(temp);

                        await ctx.SaveChangesAsync();

                    }


                }

                else
                {
                    temp.Setup(temp.Serialized!, temp.Hash!);
                }


                AST_DATA Ast = new AST_DATA(input);
                await Ast.Convert(ctx);
               
                ActiveAST Log = new ActiveAST();

                int Id = Ast.Output!.ID;

                Log.Id = Id;
                Log.FileID = input.ModelHash;
                Log.OwnerID = userId;

                Log.FPS = Ast.Output.FKR?.FPS;
                Log.Tex_Width = Ast.Output.MDL?.Width;
                Log.Tex_Height = Ast.Output.MDL?.Height;
                Log.PrecisionBits=input.PrecisionBits;
                
                await ctx.ActiveASTs.AddAsync(Log); 
                await ctx.SaveChangesAsync();



                Ast.Output.Clear();
                Ast.Output = null;
                Ast.Model?.Destructor();

                input.ModelData = null;
                input.ModelHash = null;

              

                return StatusCode(200, Id);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int id)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401);
            }

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                AST? ast = await ctx.ASTs.Include(a=>a.MDL).Include(a=>a.FKR).FirstOrDefaultAsync(a=>a.ID==id)!;
                ActiveAST? metadata = await ctx.ActiveASTs.FindAsync(ast?.ID);
                UserPreferences? owner_p = await ctx.UserPreferences.FindAsync(metadata?.OwnerID);
                bool share = owner_p!.ShareAssetOwnership;
                int ownerID = owner_p!.UserId;



                if (ast == null)
                {


                    return StatusCode(204);
                }

                else
                {
                    if (ast.MDL != null)
                    {
                        ast.MDL.Meshes = await ctx.MSHs.Where(m => m.MDL_ID == ast.MDL_ID).ToListAsync();

                        foreach(MSH m in ast.MDL.Meshes)
                        {
                            m.VT = await ctx.VTs.FindAsync(m.VT_ID);
                            m.IND= await ctx.INDs.FindAsync(m.IND_ID);
                            m.UV = await ctx.UVs.FindAsync(m.UV_ID);
                            m.NRM = await ctx.NRMs.FindAsync(m.NRM_ID);
                        }

                    }
                    if(ast.FKR != null)
                    {

                        ast.FKR.Bones=await ctx.BNs.Where(b=>b.FKR_ID== ast.FKR_ID).ToListAsync();

                        ast.FKR.Animations =await ctx.ANMs.Where(a => a.FKR_ID == ast.FKR_ID).ToListAsync();

                        foreach(ANM a in ast.FKR.Animations)
                        {
                            a.Tracks =await ctx.TKs.Where(t=>t.ANM_ID==a.ID).ToListAsync();
                        }
                    }



                    ast.Deserialize();

                    if (ast.FKR_ID != null)
                    {
                        ast.FKR!.Root = ast.FKR.Bones!.Where(b => b.Parent_ID == null).Select(b => b.ID).FirstOrDefault();
                    }


                    AssetDTO asset = new AssetDTO();

                    asset.Asset = ast;

                    asset.AST_ID = ast.ID;

                    asset.CanDelete = (userId == ownerID)||share;

                    return StatusCode(200, asset);
                }

            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401);
            }

            using (DarkforgeDBContext ctx = new DarkforgeDBContext()) { 

                AST? ast = await ctx.ASTs.FindAsync(id)!;
                ActiveAST? metadata = await ctx.ActiveASTs.FindAsync(ast?.ID);
                UserPreferences? owner_p = await ctx.UserPreferences.FindAsync(metadata?.OwnerID);

                bool share = owner_p!.ShareAssetOwnership;
                int ownerID = owner_p!.UserId;


                if (ast == null || !(share || ownerID == userId))
                {

                    return StatusCode(204);
                }


                if (ast == null)
                {
                  
                    return StatusCode(204);
                }

                else
                {

                    ctx.ASTs.Remove(ast);
                    if (ast.MDL != null)
                    {
                        ctx.MDLs.Remove(ast.MDL);

                        ctx.MSHs.RemoveRange(ast.MDL.Meshes);

                        foreach (var m in ast.MDL.Meshes)
                        {                           

                            if (m.VT != null)
                            {
                                ctx.VTs.Remove(m.VT);
                            }

                            if (m.IND != null)
                            {
                                ctx.INDs.Remove(m.IND);
                            }

                            if (m.UV != null)
                            {
                                ctx.UVs.Remove(m.UV);
                            }

                            if (m.NRM != null)
                            {
                                ctx.NRMs.Remove(m.NRM);
                            }
                        }
                    }
                    if (ast.FKR != null)
                    {
                        ctx.FKRs.Remove(ast.FKR);

                        ctx.BNs.RemoveRange(ast.FKR.Bones);
                        ctx.ANMs.RemoveRange(ast.FKR.Animations);

                        foreach (ANM a in ast.FKR.Animations)
                        {                           

                            ctx.TKs.RemoveRange(a.Tracks);
                        }

                    }




                    await ctx.SaveChangesAsync();

                    return StatusCode(200);

                }
            }

        }



    }
}

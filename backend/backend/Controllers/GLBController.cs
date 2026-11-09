
using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Logging;
using backend.Models;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GLBController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ModelDTO input)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                if (input.RPF_ID != 0)
                {
                    RPF? texture = await ctx.RPFs.AsNoTracking().FirstOrDefaultAsync(r => r.ID == input.RPF_ID);

                    if (texture != null)
                    {
                        input.TexWidth = texture.Width;
                        input.TexHeight = texture.Height;
                        input.TexOffsetX = (byte)((texture.TexX & 63) << ((texture.CLUT > 15) ? 1 : 2));
                        input.TexOffsetY = texture.TexY;
                    }
                }

                ActiveAST? Optimization = await ctx.ActiveASTs.Where(a => a.FileID == input.ModelHash).Where(a => a.PrecisionBits == input.PrecisionBits).Where(a => (a.FPS == input.TargetFPS) || a.FPS == null).FirstOrDefaultAsync();

                if (Optimization != null)
                {
                    AST? asset = await ctx.ASTs.FindAsync(Optimization.Id);

                    MDL? model = await ctx.MDLs.FindAsync(asset!.MDL_ID);


                    model!.Width = input.TexWidth;
                    model!.Height = input.TexHeight;
                    model!.OffsetX = input.TexOffsetX;
                    model!.OffsetY = input.TexOffsetY;

                    ctx.MDLs.Update(model);

                    await ctx.SaveChangesAsync();

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

                Log.FPS = Ast.Output.FKR?.FPS;
                Log.PrecisionBits = input.PrecisionBits;

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
        public async Task<IActionResult> Get([FromQuery] int id)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                AST? ast = await ctx.ASTs.Include(a => a.MDL).Include(a => a.FKR).FirstOrDefaultAsync(a => a.ID == id)!;



                if (ast == null)
                {


                    return StatusCode(204);
                }

                else
                {
                    if (ast.MDL != null)
                    {
                        ast.MDL.Meshes = await ctx.MSHs.Where(m => m.MDL_ID == ast.MDL_ID).ToListAsync();

                        foreach (MSH m in ast.MDL.Meshes)
                        {
                            m.VT = await ctx.VTs.FindAsync(m.VT_ID);
                            m.IND = await ctx.INDs.FindAsync(m.IND_ID);
                            m.UV = await ctx.UVs.FindAsync(m.UV_ID);
                            m.NRM = await ctx.NRMs.FindAsync(m.NRM_ID);
                        }

                    }
                    if (ast.FKR != null)
                    {

                        ast.FKR.Bones = await ctx.BNs.Where(b => b.FKR_ID == ast.FKR_ID).ToListAsync();

                        ast.FKR.Animations = await ctx.ANMs.Where(a => a.FKR_ID == ast.FKR_ID).ToListAsync();

                        foreach (ANM a in ast.FKR.Animations)
                        {
                            a.Tracks = await ctx.TKs.Where(t => t.ANM_ID == a.ID).ToListAsync();
                        }
                    }



                    ast.Deserialize();



                    AssetDTO asset = new AssetDTO();

                    asset.Asset = ast;

                    asset.AST_ID = ast.ID;


                    return StatusCode(200, asset);
                }

            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                AST? ast = await ctx.ASTs.FindAsync(id)!;

                if (ast == null)
                {
                    return StatusCode(204);
                }




                await ctx.ASTs.Where(a => a.ID == ast!.ID).ExecuteDeleteAsync();

                await ctx.SaveChangesAsync();

                return StatusCode(200);



            }

        }



    }
}



using backend.Converters;
using backend.Database;
using backend.Files;
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
        public IActionResult Post(ModelDTO input)
        {
            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                GLB? temp = ctx.GLBs.Where(g => g.Hash == input.ModelHash).FirstOrDefault();

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

                        ctx.GLBs.Add(temp);

                        ctx.SaveChanges();

                    }


                }

                else
                {
                    temp.Setup(temp.Serialized!, temp.Hash!);
                }


                AST_DATA Ast = new AST_DATA(input, ctx);

               

                int Id = Ast.Output!.ID;

                Ast.Output.Clear();
                Ast.Output = null;


                input.ModelData = null;
                input.ModelHash = null;

                return StatusCode(200, Id);

            }
        }

        [HttpGet]
        public IActionResult Get(int id)
        {

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                AST ast = ctx.ASTs.Include(a=>a.MDL).Include(a=>a.FKR).FirstOrDefault(a=>a.ID==id)!;
                             

                if (ast == null)
                {


                    return StatusCode(204);
                }

                else
                {
                    if (ast.MDL != null)
                    {
                        ast.MDL.Meshes = ctx.MSHs.Where(m => m.MDL_ID == ast.MDL_ID).ToList();

                        foreach(MSH m in ast.MDL.Meshes)
                        {
                            m.VT = ctx.VTs.Find(m.VT_ID);
                            m.IND= ctx.INDs.Find(m.IND_ID);
                            m.UV = ctx.UVs.Find(m.UV_ID);
                            m.NRM = ctx.NRMs.Find(m.NRM_ID);
                        }

                    }
                    if(ast.FKR != null)
                    {

                        ast.FKR.Bones=ctx.BNs.Where(b=>b.FKR_ID== ast.FKR_ID).ToList();

                        ast.FKR.Animations = ctx.ANMs.Where(a => a.FKR_ID == ast.FKR_ID).ToList();

                        foreach(ANM a in ast.FKR.Animations)
                        {
                            a.Tracks = ctx.TKs.Where(t=>t.ANM_ID==a.ID).ToList();
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

                    return StatusCode(200, asset);
                }

            }
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            using (DarkforgeDBContext ctx = new DarkforgeDBContext()) { 

                AST ast = ctx.ASTs.Find(id)!;

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




                    ctx.SaveChanges();

                    return StatusCode(200);

                }
            }

        }



    }
}

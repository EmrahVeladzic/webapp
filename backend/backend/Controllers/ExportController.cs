using backend.Database;
using backend.Models;
using backend.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {

        [HttpPost]
        public int Post(ExportDTO exportDTO)
        {
            return 0;
        }


        [HttpGet]
        public async Task<IActionResult>Get(int ast_id, int rpf_id, int wl_id)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401);
            }


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


                ExportDTO export = new ExportDTO();

                AST? ast = await ctx.ASTs.Include(a => a.MDL).Include(a => a.FKR).FirstOrDefaultAsync(a => a.ID == ast_id)!;

                if (ast != null)
                {
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

                    }


                    ast.Deserialize();

                    export.AST = Convert.ToBase64String(ast.ToArrayBuffer());

                    ast.Clear();

                }


                RPF? rpf = await ctx.RPFs.Include(r => r.PLT).Include(r => r.PGA).FirstOrDefaultAsync(r => r.ID == rpf_id)!;

                if (rpf != null)
                {

                    rpf.Deserialize();

                    export.RPF = Convert.ToBase64String(rpf.ToArrayBuffer());

                    rpf.Clear();
                }

                WL? wl = await ctx.WLs.FindAsync(wl_id)!;

                if (wl != null)
                {

                    wl.Deserialize();

                    export.WL = Convert.ToBase64String(wl.ToArrayBuffer());

                    wl.Clear();
                }                

               

                return StatusCode(200,export);

            }       


        }

    }
}

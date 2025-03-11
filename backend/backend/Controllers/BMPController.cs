using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Logging;
using backend.Models;
using backend.Requests;
using backend.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BMPController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ImageDTO input)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401);
            }


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


                ActiveRPF? Optimization = await ctx.ActiveRPFs.Where(a=>a.FileID==input.ImageHash).Where(a=>a.CLUT==input.CLUT_Size && a.Method==input.Mode && a.ProtectedBufferSize==input.ProtectedBufferSize).Where(a=>(a.AlphaPresent==false && input.Alpha == null) || (input.Alpha!=null && input.Alpha[0]==a.Red && input.Alpha[1]==a.Green && input.Alpha[2]==a.Blue)).FirstOrDefaultAsync();

                if (Optimization != null)
                {
                    return StatusCode(200, Optimization.Id);
                }


                BMP? temp = await ctx.BMPs.Where(b => b.Hash == input.ImageHash).FirstOrDefaultAsync();

                if (temp == null)
                {
                    if (input.ImageData == null)
                    {

                        return StatusCode(204);
                    }

                    else
                    {


                        StringBuilder stringBuilder = new StringBuilder(input.ImageData!, input.ImageData!.Length);

                        stringBuilder.Replace("\r\n", String.Empty);
                        stringBuilder.Replace(" ", String.Empty);
                        stringBuilder.Replace("data:image/bmp;base64,", String.Empty);


                        byte[] Data = System.Convert.FromBase64String(stringBuilder.ToString());

                        temp = new BMP();

                        temp!.Setup(Data, input!.ImageHash!);

                        await ctx.BMPs.AddAsync(temp);

                        await ctx.SaveChangesAsync();

                    }
                }

                else
                {
                    temp.Setup(temp.Serialized!, temp.Hash!);
                }

                IMG_DATA Img = new IMG_DATA(input);

                await Img.Convert(ctx);

                ActiveRPF Log = new ActiveRPF();

                int Id = Img.Output!.ID;

                Log.Id = Id;
                Log.FileID = input.ImageHash;
                Log.OwnerID = userId;

                Log.CLUT = input.CLUT_Size;
                Log.ProtectedBufferSize = input.ProtectedBufferSize;
                Log.Method=input.Mode;

                
                Log.Red = Img.Alpha?.Red;
                Log.Green = Img.Alpha?.Green;
                Log.Blue = Img.Alpha?.Blue;

                Log.AlphaPresent = Img.AlphaUsed;

                await ctx.ActiveRPFs.AddAsync(Log);
                
                await ctx.SaveChangesAsync();
               
               

                Img.Output.Clear();
                Img.Output = null;
                Img.Image?.Destructor();



                input.ImageData = null;
                input.ImageHash = null;



               


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

                RPF? rpf = await ctx.RPFs.Include(r=>r.PLT).Include(r=>r.PGA).FirstOrDefaultAsync(r=>r.ID==id)!;
                ActiveRPF? metadata = await ctx.ActiveRPFs.FindAsync(rpf?.ID);
                UserPreferences? owner_p = await ctx.UserPreferences.FindAsync(metadata?.OwnerID);

                bool share = owner_p!.ShareAssetOwnership;
                int ownerID = owner_p!.UserId;

                if (rpf == null){

                    return StatusCode(204);
                }

                else
                {
                    TextureDTO texture = new TextureDTO();

                    texture.Width = rpf.Width;
                    texture.Height = rpf.Height;
                    texture.Colours = rpf.CLUT;
                    texture.Pixels = rpf.PGA?.Serialized!.ToList();
                    rpf.PLT!.Deserialize();
                    texture.CLUT = rpf.PLT?.Data!.Select(d => d.Data).ToList();
                    texture.RPF_ID = rpf.ID;

                    texture.CanDelete = (ownerID==userId)||share;

                    return StatusCode(200, texture);
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

            using (DarkforgeDBContext ctx = new DarkforgeDBContext()){

                RPF? rpf = await ctx.RPFs.Include(r => r.PLT).Include(r => r.PGA).FirstOrDefaultAsync(r => r.ID == id)!;

                if (rpf == null)
                {
                    return StatusCode(204);
                }

                ActiveRPF? metadata = await ctx.ActiveRPFs.FindAsync(rpf?.ID);
                UserPreferences? owner_p = await ctx.UserPreferences.FindAsync(metadata?.OwnerID);

                bool share = owner_p!.ShareAssetOwnership;
                int ownerID = owner_p!.UserId;


                if (!(share||userId==ownerID))
                {                   
                    return StatusCode(204);
                }

                else
                {

                    await ctx.Database.ExecuteSqlRawAsync("DELETE FROM Models.RPF WHERE EntityID = {0}", rpf!.ID);

                    await ctx.SaveChangesAsync();
                    

                    return StatusCode(200);
                }

            }
        }

    }

   

    
}

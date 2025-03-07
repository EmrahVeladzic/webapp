using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMPController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(ImageDTO input)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


                BMP? temp = ctx.BMPs.Where(b => b.Hash == input.ImageHash).FirstOrDefault();

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

                        ctx.BMPs.Add(temp);

                        ctx.SaveChanges();

                    }
                }

                else
                {
                    temp.Setup(temp.Serialized!, temp.Hash!);
                }

                IMG_DATA Img = new IMG_DATA(input, ctx);



                int Id = Img.Output!.ID;

                Img.Output.Clear();
                Img.Output = null;




                input.ImageData = null;
                input.ImageHash = null;

                return StatusCode(200, Id);

            }
        }


        [HttpGet]
        public IActionResult Get(int id)
        {

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                RPF rpf = ctx.RPFs.Include(r=>r.PLT).Include(r=>r.PGA).FirstOrDefault(r=>r.ID==id)!;

                

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

                    return StatusCode(200, texture);
                }

            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            using (DarkforgeDBContext ctx = new DarkforgeDBContext()){

                RPF rpf = ctx.RPFs.Include(r => r.PLT).Include(r => r.PGA).FirstOrDefault(r => r.ID == id)!;


                if (rpf == null)
                {                   
                    return StatusCode(204);
                }

                else
                {
                    ctx.RPFs.Remove(rpf);
                    ctx.PGAs.Remove(rpf.PGA!);
                    ctx.PLTs.Remove(rpf.PLT!);

                    ctx.SaveChanges();

                    

                    return StatusCode(200);
                }

            }
        }

    }

   

    
}

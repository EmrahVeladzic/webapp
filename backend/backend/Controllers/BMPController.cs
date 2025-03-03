using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMPController : ControllerBase
    {
      
        [HttpPost]
        public IActionResult Post(ImageJson input)
        {


            DarkforgeDBContext ctx = new DarkforgeDBContext();

            BMP? temp = ctx.BMPs.Where(b=>b.Hash==input.ImageHash).FirstOrDefault();

            if(temp == null)            
            {
                if (input.ImageData == null)
                {

                    return StatusCode(404);
                }

                else {


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
          
                    

            IMG_DATA Img = new IMG_DATA(input,ctx);

            ctx.Dispose();

            return StatusCode(200,Img.Texture);
        }


    }
}

using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WAVController : ControllerBase
    {


        [HttpPost]
        public IActionResult Post(SoundJson input)
        {


            DarkforgeDBContext ctx = new DarkforgeDBContext();

            WAV? temp = ctx.WAVs.Where(b => b.Hash == input.SoundHash).FirstOrDefault();

            if (temp == null)
            {

                if (input.SoundData == null)
                {
                    return StatusCode(404);

                }

                else
                {

                    StringBuilder stringBuilder = new StringBuilder(input.SoundData!, input.SoundData!.Length);

                    stringBuilder.Replace("\r\n", String.Empty);
                    stringBuilder.Replace(" ", String.Empty);
                    stringBuilder.Replace("data:audio/wav;base64,", String.Empty);


                    byte[] Data = System.Convert.FromBase64String(stringBuilder.ToString());

                    temp = new WAV();

                    temp!.Setup(Data, input!.SoundHash!);

                    ctx.WAVs.Add(temp);

                    ctx.SaveChanges();

                }
            }


            
            SFX_DATA Sfx = new SFX_DATA(input,ctx);

            ctx.Dispose();


            return StatusCode(200,Sfx.Audio);
        }
    }
}

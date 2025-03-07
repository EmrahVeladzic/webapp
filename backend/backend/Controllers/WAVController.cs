using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Models;
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
        public IActionResult Post(SoundDTO input)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                WAV? temp = ctx.WAVs.Where(b => b.Hash == input.SoundHash).FirstOrDefault();

                if (temp == null)
                {

                    if (input.SoundData == null)
                    {


                        return StatusCode(204);

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

                else
                {
                    temp.Setup(temp.Serialized!, temp.Hash!);
                }

                SFX_DATA Sfx = new SFX_DATA(input, ctx);



                int Id = Sfx.Output!.ID;

                Sfx.Output.Clear();


                input.SoundData = null;
                input.SoundHash = null;

                return StatusCode(200, Id);

            }
        }


        [HttpGet]
        public IActionResult Get(int id)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


                WL wl = ctx.WLs.Find(id)!;


                if (wl == null)
                {


                    return StatusCode(204);
                }

                else
                {

                    wl.Deserialize();

                    AudioDTO audio = new AudioDTO();

                    audio.AudioData = wl.Serialized!.ToList();

                    wl.Serialized = null;

                    audio.WL_ID = wl.ID;

                    audio.SampleRate = wl.SampleRate;

                    audio.ThresholdBits = wl.ThresholdBits;

                    audio.BlockCountPerChannel = (UInt32)wl.BlockCountPerChannel;

                    audio.ChannelCount = wl.ChannelCount;

                    return StatusCode(200, audio);
                }

            }


        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


                WL wl = ctx.WLs.Find(id)!;



                if (wl == null)
                {

                    return StatusCode(204);
                }

                else
                {
                    ctx.WLs.Remove(wl);

                    ctx.SaveChanges();


                    return StatusCode(200);
                }

            }

        }

    }
}

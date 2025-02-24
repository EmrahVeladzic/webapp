using backend.Converters;
using backend.Database;
using backend.Files;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GLBController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(ModelJson input)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            GLB? temp =  ctx.GLBs.Where(g=>g.Hash==input.ModelHash).FirstOrDefault();

            if (temp == null)
            {
                if (input.ModelData == null)
                {

                    return StatusCode(404);

                }

                else
                {
                    StringBuilder stringBuilder = new StringBuilder(input.ModelData!, input.ModelData!.Length);

                    stringBuilder.Replace("\r\n", String.Empty);
                    stringBuilder.Replace(" ", String.Empty);
                    stringBuilder.Replace("data:application/octet-stream;base64,", String.Empty);

                    byte[] Data = System.Convert.FromBase64String(stringBuilder.ToString());

                    temp = new GLB();

                    temp!.Setup(Data,input!.ModelHash!);

                    ctx.GLBs.Add(temp);

                    ctx.SaveChanges();

                }


            }

            ctx.Dispose();

            AST_DATA Ast = new AST_DATA(input); 

            return StatusCode(200,Ast.Output);
        }

      
    }
}

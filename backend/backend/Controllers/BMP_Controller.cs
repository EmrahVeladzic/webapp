using backend.Database;
using backend.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMP_Controller : ControllerBase
    {
        [HttpPost]
        public void Post(ImageJson input)
        {
            
        }

        [HttpGet]
        public void GetMagic(int id)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            UInt16 k = (UInt16)(ctx.BMPs.Where(b=>b.Id == id).Select(b => b.Magic).FirstOrDefault());

            Console.WriteLine(k);

            ctx.Dispose();
        }
    }
}

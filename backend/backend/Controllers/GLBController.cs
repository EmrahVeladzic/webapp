using backend.Files;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GLBController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(ModelJson input)
        {


            return StatusCode(200);
        }

      
    }
}

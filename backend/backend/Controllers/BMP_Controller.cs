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
    }
}

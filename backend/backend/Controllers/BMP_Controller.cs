using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BMP_Controller : Controller
    {
        private readonly IBMP_Repository repository;

        public BMP_Controller(IBMP_Repository rp)
        {
            this.repository = rp;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadImage([FromBody] string data)
        {
            if (data == null)
            {
                

                return BadRequest(ModelState);
            }

            else
            {
               
                byte[] bytes = Convert.FromBase64String(data);


                repository.UploadBMP(bytes);

                

                return NoContent();
            }
        }


    }
}

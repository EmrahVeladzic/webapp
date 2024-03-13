using backend.Interfaces;
using backend.Models;
using backend.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;



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
        public IActionResult UploadImage([FromBody] ImageJson J_Object)
        {

            
            if (J_Object == null)
            {               

                return BadRequest(ModelState);
            }

            
            else
            {
                                

                string sha1 = string.Empty;
                
                byte[] bytes = Convert.FromBase64String(J_Object.ImageData!);

                using (SHA1 sh =SHA1.Create())
                {
                    if (bytes != null)
                    {
                        byte[] hashbyte = sh.ComputeHash(bytes);
                        sha1 = BitConverter.ToString(hashbyte, 0, hashbyte.Length);
                    }
                }

                if (bytes != null)
                {

                    repository.UploadBMP(bytes, sha1);

                }

                return NoContent();
            }
        }


    }
}

using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RPF_Controller : Controller
    {

        private readonly IRPF_Repository repository;

        public RPF_Controller(IRPF_Repository rp)
        {
            this.repository = rp;
        }


        
    }
}

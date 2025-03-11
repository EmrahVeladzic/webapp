using backend.Database;
using backend.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId) || userId != id)
            {
                return StatusCode(401); 
            }


            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                UserPreferences? prefs = await ctx.UserPreferences.FindAsync(id);

                return StatusCode(200, prefs);

            }


        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserPreferences prefs)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId) || userId != prefs.UserId)
            {
                return StatusCode(401);
            }

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                UserPreferences? original = await ctx.UserPreferences.FindAsync(prefs.UserId);

                if (original != null)
                {
                    ctx.UserPreferences.Entry(original).CurrentValues.SetValues(prefs);

                    

                    await ctx.SaveChangesAsync();

                    return StatusCode(200);


                }

                else
                {
                    return StatusCode(204);
                }
               

            }


        }

    }
}

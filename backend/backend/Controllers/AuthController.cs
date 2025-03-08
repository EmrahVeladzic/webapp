using backend.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Users;
using Microsoft.EntityFrameworkCore;
using backend.Requests;


namespace backend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       
        private readonly IConfiguration cfg;
        private readonly PasswordHasher<UserAccount> _passwordHasher;

        public AuthController(IConfiguration config)
        {
            
            cfg = config;
            _passwordHasher = new PasswordHasher<UserAccount>();
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromBody] LogInRequest request)
        {
         

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {
                UserAccount? user = await ctx.Users.FirstOrDefaultAsync(u=>u.Username==request.Username);

                if (user == null)
                {
                    return StatusCode(401);
                }

                else
                {

                    UserPreferences? pref = await ctx.UserPreferences.FirstOrDefaultAsync(p => p.UserId == user!.ID);


                    PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password!);
                    if (result == PasswordVerificationResult.Failed)
                    {
                        return StatusCode(401);
                    }



                    string token = GenerateJwtToken(user,pref!);
                    return StatusCode(200, new { token });
                }
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> GenerateUser([FromBody] SignUpRequest request)
        {
            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {

                var existingUser = await ctx.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (existingUser != null)
                {
                    return StatusCode(400);
                }

                UserAccount user = new UserAccount(request.Username!, _passwordHasher.HashPassword(null!, request.Password!));

                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                UserPreferences prefs = new UserPreferences(user.ID, request.Language!,request.Lifespan,request.SharedAssets);

                await ctx.UserPreferences.AddAsync(prefs);
                await ctx.SaveChangesAsync();


                return StatusCode(200);

            }
        }

        private string GenerateJwtToken(UserAccount user, UserPreferences pref)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
              
            };

            JwtSecurityToken token = new JwtSecurityToken(
                cfg["Jwt:Issuer"],
                cfg["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    



    }
}

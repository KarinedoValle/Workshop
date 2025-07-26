using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Workshop.DB;
using Workshop.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SecurityKey _jwtKey;

        public AutorizacaoController(AppDbContext context, SecurityKey jwtKey)
        {
            _context = context;
            _jwtKey = jwtKey;
        }

        [HttpPost("token")]
        public IActionResult Token([FromForm] Token request)
        {
            var Usuario = _context.Usuario.FirstOrDefault(i => i.Perfil == request.Profile);

            if (Usuario == null)
            {
                return Unauthorized(new { error = "Perfil inválido." });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Usuario.Login),
                new Claim("Cpf", Usuario.Cpf.ToString()),
                new Claim("Perfil", Usuario.Perfil)
            };

            var creds = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5001",
                audience: "http://localhost:5001",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                expires_in = 3600
            });
        }
    }

    


}

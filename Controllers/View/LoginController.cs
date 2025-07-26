using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Workshop.DB;
using Workshop.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;


namespace Workshop.Controllers.View
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly SecurityKey _jwtKey;

        public LoginController(ILogger<HomeController> logger, AppDbContext context, SecurityKey jwtKey)
        {
            _logger = logger;
            _context = context;
            _jwtKey = jwtKey;
        }

        public IActionResult Logar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logar(string username, string password)
        {
            var Usuario = _context.Usuario.FirstOrDefault(i => i.Login == username);

            if (Usuario == null)
            {
                ViewBag.Message = "Usuário ou senha inválidos";
                return View();
            }

            var hasher = new PasswordHasher<Usuario>();
            var result = hasher.VerifyHashedPassword(Usuario, Usuario.Senha, password);

            if (result == PasswordVerificationResult.Success) {

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
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                TempData["Token"] = tokenString;

                Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
                {
                    HttpOnly = false, 
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

                return RedirectToAction("Workshops", "Home");
            }
            
            

            ViewBag.Message = "Usuário ou senha inválidos";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Logar", "Login");
        }

    }
}

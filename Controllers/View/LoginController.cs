using Microsoft.AspNetCore.Mvc;

namespace Workshop.Controllers.View
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        public IActionResult Logar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logar(string username, string password)
        {
            var (sucesso, token, erro) = _loginService.Autenticar(username, password);

            if (!sucesso)
            {
                ViewBag.Message = erro;
                return View();
            }

            TempData["Token"] = token;

            Response.Cookies.Append("AuthToken", token!, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return RedirectToAction("Workshops", "Home");
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

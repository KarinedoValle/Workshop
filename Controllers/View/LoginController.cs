using Microsoft.AspNetCore.Mvc;

namespace Workshop.Controllers.View
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public LoginController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Usuário ou senha inválidos";
                return View();
            }
        }
    }
}

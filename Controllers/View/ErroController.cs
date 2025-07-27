using Microsoft.AspNetCore.Mvc;

namespace Workshop.Controllers.View
{
    public class ErroController : Controller
    {
        private readonly ILogger<ErroController> _logger;
        public ErroController(ILogger<ErroController> logger)
        {
            _logger = logger;
        }

        [Route("Erro/{statusCode}")]
        public IActionResult Erro(int statusCode)
        {
            switch (statusCode)
            {
                case 401: return View("401");
                default: return View("401");
            }
        }
    }
}

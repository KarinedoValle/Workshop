using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.DB;
using Workshop.Enums;

namespace Workshop.Controllers.View
{
    [Authorize]
    [Route("Instrutor")]
    public class InstrutrController : Controller
    {
        private readonly ILogger<InstrutrController> _logger;
        private readonly AppDbContext _context;
        public InstrutrController(ILogger<InstrutrController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Cadastrar")]
        public IActionResult Cadastrar()
        {
            ViewBag.Perfis = EnumExtensions.GetEnumDescriptions<Perfil>();
            return View();
        }

        [HttpGet("Instrutores")]
        public IActionResult Instrutores()
        {
            return View();
        }

        [HttpGet("Instrutor/{cpf}")]
        public IActionResult Instrutor(string cpf)
        {
            Models.Instrutor? Instrutor = _context.Instrutor.FirstOrDefault(C => C.Cpf == cpf);
            
            if (Instrutor == null)
                return NotFound();
            ViewBag.Perfis = EnumExtensions.GetEnumDescriptions<Perfil>();
            return View(Instrutor);
        }

    }
}

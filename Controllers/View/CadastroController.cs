using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workshop.DB;
using Workshop.Enums;
using Workshop.Models;

namespace Workshop.Controllers.View
{
    [Authorize]
    [Route("Cadastro")]
    public class CadastroController : Controller
    {
        private readonly ILogger<CadastroController> _logger;
        private readonly AppDbContext _context;
        public CadastroController(ILogger<CadastroController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Cadastrar")]
        public IActionResult Cadastrar()
        {

            ViewBag.Categorias = EnumExtensions.GetEnumDescriptions<Categoria>();
            ViewBag.Modalidades = EnumExtensions.GetEnumDescriptions<Modalidade>(); 
            ViewBag.Status = EnumExtensions.GetEnumDescriptions<Status>();

            List<Instrutor> Instrutores = _context.Instrutor.ToList();
            ViewBag.Instrutores = Instrutores;

            return View();
        }

        [HttpGet("Workshop/{id}")]
        public IActionResult Workshop(int id)
        {
            Models.Workshop? workshop = _context.Workshop.Include(w => w.Instrutor).FirstOrDefault(w => w.ID == id);
            
            if (workshop == null)
                return NotFound();

            ViewBag.Categorias = EnumExtensions.GetEnumDescriptions<Categoria>();
            ViewBag.Modalidades = EnumExtensions.GetEnumDescriptions<Modalidade>();
            ViewBag.Status = EnumExtensions.GetEnumDescriptions<Status>();
            ViewBag.Instrutores = _context.Instrutor.ToList();
            return View(workshop);
        }
    }
}

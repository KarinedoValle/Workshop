using Microsoft.AspNetCore.Mvc;
using Workshop.DB;

namespace Workshop.Controllers.View
{
    [Route("Colaborador")]
    public class ColaboradorController : Controller
    {
        private readonly ILogger<ColaboradorController> _logger;
        private readonly AppDbContext _context;
        public ColaboradorController(ILogger<ColaboradorController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Get/{cpf}")]
        public IActionResult Get(string cpf)
        {
            Models.Instrutor? Instrutor = _context.Instrutor.FirstOrDefault(C => C.Cpf == cpf);
            
            if (Instrutor == null)
                return NotFound();

            return View(Instrutor);
        }
    }
}

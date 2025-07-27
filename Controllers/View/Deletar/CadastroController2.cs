//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Workshop.DB;
//using Workshop.Enums;
//using Workshop.Models;

//namespace Workshop.Controllers.View
//{
//    [Authorize]
//    [Route("Cadastro")]
//    public class CadastroController2 : Controller
//    {
//        private readonly ILogger<CadastroController2> _logger;
//        private readonly AppDbContext _context;
//        public CadastroController2(ILogger<CadastroController2> logger, AppDbContext context)
//        {
//            _logger = logger;
//            _context = context;
//        }

//        [HttpGet("Cadastrar")]
//        public IActionResult Cadastrar()
//        {
//            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
//            if (!admin)
//                return Forbid();

//            ViewBag.Categorias = EnumExtensions.GetEnumDescriptions<Categoria>();
//            ViewBag.Modalidades = EnumExtensions.GetEnumDescriptions<Modalidade>(); 
//            ViewBag.Status = EnumExtensions.GetEnumDescriptions<Status>();

//            List<Usuario> Usuarios = _context.Usuario.ToList();
//            ViewBag.Usuarios = Usuarios;

//            return View();
//        }

//        [HttpGet("Workshop/{id}")]
//        public IActionResult Workshop(int id)
//        {
//            Models.Workshop? workshop = _context.Workshop.Include(w => w.Usuario).FirstOrDefault(w => w.ID == id);

//            if (workshop == null)
//                return NotFound();

//            var userCpf = User.FindFirst("Cpf")?.Value;
//            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
//            bool proprioCPF = User.FindFirst("Cpf")?.Value == Models.Usuario.FormatCpf(workshop.Usuario.Cpf);
//            if (!admin && !proprioCPF)
//            {
//                return Forbid();
//            }

//            ViewBag.Categorias = EnumExtensions.GetEnumDescriptions<Categoria>();
//            ViewBag.Modalidades = EnumExtensions.GetEnumDescriptions<Modalidade>();
//            ViewBag.Status = EnumExtensions.GetEnumDescriptions<Status>();
//            ViewBag.Usuarios = _context.Usuario.ToList();
//            return View(workshop);
//        }
//    }
//}

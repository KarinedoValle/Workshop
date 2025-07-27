//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Workshop.DB;
//using Workshop.Enums;

//namespace Workshop.Controllers.View.Deletar
//{
//    [Authorize]
//    [Route("Usuario")]
//    public class UsuarioController2 : Controller
//    {
//        private readonly ILogger<UsuarioController2> _logger;
//        private readonly AppDbContext _context;
//        public UsuarioController2(ILogger<UsuarioController2> logger, AppDbContext context)
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


//            ViewBag.Perfis = EnumExtensions.GetEnumDescriptions<Perfil>();
//            return View();
//        }

//        [HttpGet("Usuarios")]
//        public IActionResult Usuarios()
//        {
//            return View();
//        }

//        [HttpGet("Usuario/{cpf}")]
//        public IActionResult Usuario(string cpf)
//        {
//            var userCpf = User.FindFirst("Cpf")?.Value;
//            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());

//            if (!admin && cpf != userCpf)
//                return Forbid();


//            Models.Usuario? Usuario = _context.Usuario.FirstOrDefault(C => C.Cpf == cpf);

//            if (Usuario == null)
//                return NotFound();
//            ViewBag.Perfis = EnumExtensions.GetEnumDescriptions<Perfil>();
//            return View(Usuario);
//        }

//    }
//}

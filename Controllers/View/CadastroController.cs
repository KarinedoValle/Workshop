using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.Enums;
using Workshop.Models;

namespace Workshop.Controllers.View
{
    [Authorize]
    [Route("Cadastro")]
    public class CadastroController : Controller
    {
        private readonly ILogger<CadastroController> _logger;
        private readonly ICadastroService _cadastroService;

        public CadastroController(ILogger<CadastroController> logger, ICadastroService cadastroService)
        {
            _logger = logger;
            _cadastroService = cadastroService;
        }

        [HttpGet("Cadastrar")]
        public IActionResult Cadastrar()
        {
            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
            if (!admin)
                return Forbid();

            ViewBag.Categorias = _cadastroService.ObterCategorias();
            ViewBag.Modalidades = _cadastroService.ObterModalidades();
            ViewBag.Status = _cadastroService.ObterStatus();

            List<Usuario> Usuarios = _cadastroService.ObterUsuarios();
            ViewBag.Usuarios = Usuarios;

            return View();
        }

        [HttpGet("Workshop/{id}")]
        public IActionResult Workshop(int id)
        {
            var workshop = _cadastroService.ObterWorkshop(id);

            if (workshop == null)
                return NotFound();

            var userCpf = User.FindFirst("Cpf")?.Value;
            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            bool proprioCPF = userCpf == Models.Usuario.FormatCpf(workshop.Usuario.Cpf);

            if (!admin && !proprioCPF)
            {
                return Forbid();
            }

            ViewBag.Categorias = _cadastroService.ObterCategorias();
            ViewBag.Modalidades = _cadastroService.ObterModalidades();
            ViewBag.Status = _cadastroService.ObterStatus();
            ViewBag.Usuarios = _cadastroService.ObterUsuarios();

            return View(workshop);
        }
    }
}

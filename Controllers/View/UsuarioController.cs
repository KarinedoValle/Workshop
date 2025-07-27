using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.Services.View.Usuario;

namespace Workshop.Controllers.View
{
    [Authorize]
    [Route("Usuario")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpGet("Cadastrar")]
        public IActionResult Cadastrar()
        {
            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            if (!admin)
                return Forbid();

            ViewBag.Perfis = _usuarioService.ObterPerfis();
            return View();
        }

        [HttpGet("Usuarios")]
        public IActionResult Usuarios()
        {
            return View();
        }

        [HttpGet("Usuario/{cpf}")]
        public IActionResult Usuario(string cpf)
        {
            var userCpf = User.FindFirst("Cpf")?.Value;
            bool admin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());

            if (!admin && !_usuarioService.UsuarioTemPermissao(userCpf, cpf))
                return Forbid();

            var usuario = _usuarioService.ObterUsuarioPorCpf(cpf);

            if (usuario == null)
                return NotFound();

            ViewBag.Perfis = _usuarioService.ObterPerfis();
            return View(usuario);
        }
    }
}

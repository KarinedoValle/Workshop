using Microsoft.AspNetCore.Mvc;
using Workshop.Models;

namespace Workshop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutorizacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromForm] Token request)
        {
            var (sucesso, resultado, erro) = _autenticacaoService.GerarToken(request);

            if (!sucesso)
                return Unauthorized(new { error = erro });

            return Ok(resultado);
        }
    }
}

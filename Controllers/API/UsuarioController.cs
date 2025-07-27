using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.Models;
using Workshop.Enums;
using Workshop.Services.API.Usuario;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var user = HttpContext.User;
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
        string? cpf = user.FindFirst("Cpf")?.Value;

        var result = _service.ObterTodos(isAdmin, cpf);
        return Ok(result);
    }

    [HttpGet("{cpf}")]
    public IActionResult Get(string cpf)
    {
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
        string? cpfUsuario = User.FindFirst("Cpf")?.Value;
        var (ok, resultado, erro) = _service.ObterPorCpf(cpf, cpfUsuario, isAdmin);

        if (!ok)
            return erro == null ? NotFound() : StatusCode(403, erro);
        return Ok(resultado);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Usuario usuario)
    {
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
        var (ok, erro, criado) = _service.Criar(usuario, isAdmin);

        if (!ok)
            return BadRequest(erro);

        return CreatedAtAction(nameof(Get), new { cpf = usuario.Cpf }, criado);
    }

    [HttpPut("{cpf}")]
    public IActionResult Put(string cpf, [FromBody] Usuario usuario)
    {
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
        string? cpfUsuario = User.FindFirst("Cpf")?.Value;
        string? perfilUsuario = User.FindFirst("Perfil")?.Value;

        var (ok, erro) = _service.Atualizar(cpf, usuario, isAdmin, cpfUsuario, perfilUsuario);

        if (!ok)
            return BadRequest(erro);
        return Ok();
    }

    [HttpDelete("{cpf}")]
    public IActionResult Delete(string cpf)
    {
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());
        var (ok, erro) = _service.Deletar(cpf, isAdmin);

        if (!ok)
            return erro != null ? BadRequest(erro) : NotFound();

        return Ok();
    }
}

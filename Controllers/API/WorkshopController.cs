using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workshop.Enums;
using Workshop.Models;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WorkshopController : ControllerBase
{
    private readonly IWorkshopService _service;

    public WorkshopController(IWorkshopService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get() => Ok(_service.ObterTodos());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var cpf = User.FindFirst("Cpf")?.Value;
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());

        var workshop = _service.ObterPorId(id, cpf, isAdmin, out string? erroPermissao);
        if (workshop == null)
            return erroPermissao != null
                ? StatusCode(403, erroPermissao)
                : NotFound();

        return Ok(workshop);
    }

    [HttpPost]
    public IActionResult Post([FromBody] WorkshopRequest request)
    {
        var cpf = User.FindFirst("Cpf")?.Value;
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());

        var (sucesso, erro, workshop) = _service.Criar(request, cpf, isAdmin);
        if (!sucesso)
            return BadRequest(erro);

        return CreatedAtAction(nameof(Get), new { id = workshop!.ID }, workshop);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] WorkshopRequest request)
    {
        var cpf = User.FindFirst("Cpf")?.Value;
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());

        var (sucesso, erro) = _service.Atualizar(id, request, cpf, isAdmin);
        if (!sucesso)
            return BadRequest(erro);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        bool isAdmin = User.HasClaim(c => c.Type == "Perfil" && c.Value == Perfil.Administrador.GetDescription());

        var (sucesso, erro) = _service.Deletar(id, isAdmin);
        if (!sucesso)
            return erro != null ? StatusCode(403, erro) : NotFound();

        return Ok();
    }
}

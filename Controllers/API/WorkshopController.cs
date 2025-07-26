using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Workshop.DB;
using Workshop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkshopController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]

        public ActionResult<IEnumerable<Models.Workshop>> Get()
        {

            List<Models.Workshop> workshops = _context.Workshop
             .Include(w => w.Usuario)
             .ToList();

            

            return Ok(Models.Workshop.Sort(workshops));

        }


        [HttpGet("{id}")]
        public ActionResult<Models.Workshop> Get(int id)
        {
            Models.Workshop? workshop = _context.Workshop
             .Include(w => w.Usuario)
             .FirstOrDefault(w => w.ID == id);

            if (workshop == null)
                return NotFound();

            var user = HttpContext.User;
            bool isAdmin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            bool proprioCPF = User.FindFirst("Cpf")?.Value == Models.Usuario.FormatCpf(workshop.Usuario.Cpf);
            if (!proprioCPF && !isAdmin)
                return new ObjectResult("Sem permissão para visualizar este workshop.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            return Ok(workshop);

        }

        [HttpPost]
        public ActionResult Post([FromBody] WorkshopRequest workshop)
        {
            var user = HttpContext.User;
            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
                return new ObjectResult("Sem permissão para cadastrar workshops.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Usuario? Usuario = _context.Usuario.FirstOrDefault(Usuario => Usuario.Cpf == Usuario.FormatCpf(workshop.UsuarioCpf));
            if (Usuario == null)
                return BadRequest($"Usuario com CPF {workshop.UsuarioCpf} não encontrado.");

            if(Usuario.Perfil == Enums.Perfil.Leitor.GetDescription())
                return BadRequest($"Não é possível cadastrar workshops com um instrutor que tenha perfil 'Leitor'.");

            Models.Workshop workshopModelo = Models.Workshop.ConverteParaModelo(workshop, Usuario);

            if(!ValidarHorarioComercial(workshopModelo.Datas))
                return BadRequest("Não é possível cadastrar encontros fora do horário comercial (Entre 8h e 19h)");

            workshopModelo.Status = AtualizarStatus(workshopModelo);
            _context.Workshop.Add(workshopModelo);

             _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = workshopModelo.ID }, workshopModelo);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] WorkshopRequest workshop)
        {
            var user = HttpContext.User;
            bool isAdmin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            bool proprioCPF = User.FindFirst("Cpf")?.Value == Models.Usuario.FormatCpf(workshop.UsuarioCpf);
            if (!proprioCPF && !isAdmin)
                return new ObjectResult("Sem permissão para editar este workshop.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.Workshop? workshopEncontrado = _context.Workshop.Find(id);
            Models.Workshop? workshopl = _context.Workshop
             .Include(w => w.Usuario)
             .FirstOrDefault(w => w.ID == id);
            if (workshopEncontrado == null)
                return NotFound($"Workshop com Id {id} não encontrado.");

            if(workshopEncontrado.Status == Enums.Status.Concluido.GetDescription())
                return BadRequest($"Não é possível editar workshops com status concluído.");

            bool instrutorAlterado = workshopEncontrado.Usuario.Cpf != Models.Usuario.FormatCpf(workshop.UsuarioCpf);
            if (proprioCPF && instrutorAlterado && !isAdmin)
                return BadRequest($"Usuário sem permissão para alterar o instrutor do workshop.");

            Usuario? Usuario = _context.Usuario.FirstOrDefault(Usuario => Usuario.Cpf == Usuario.FormatCpf(workshop.UsuarioCpf));
            if (Usuario == null)
                return BadRequest($"Usuario com CPF {workshop.UsuarioCpf} não encontrado.");

            Models.Workshop workshopModelo = Models.Workshop.ConverteParaModelo(workshop, Usuario, workshopEncontrado.ID);

            if (!ValidarHorarioComercial(workshopModelo.Datas))
                return BadRequest("Não é possível cadastrar encontros fora do horário comercial (Entre 8h e 19h)");

            workshopModelo.Status = AtualizarStatus(workshopModelo);

            _context.Entry(workshopEncontrado).State = EntityState.Detached;

            _context.Entry(workshopModelo).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = HttpContext.User;
            bool isAdmin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            if (!isAdmin)
                return new ObjectResult("Usuário sem permissão para deletar workshops.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            Models.Workshop? workshop = _context.Workshop.Find(id);
            if (workshop == null)
                return NotFound();


            _context.Workshop.Remove(workshop);
            _context.SaveChanges();
            return NoContent();

        }

        private bool ValidarHorarioComercial(List<DateTime>? datas)
        {
            if (datas == null || datas.Count == 0)
                return true;

            return !datas.Any(data => data.TimeOfDay < TimeSpan.FromHours(8) || data.TimeOfDay > TimeSpan.FromHours(19));
        }


        private string AtualizarStatus(Models.Workshop workshopModelo)
        {
            if (workshopModelo.Datas.Count == 0)
                return Enums.Status.Novo.GetDescription();

            bool datasFuturas = workshopModelo.Datas.Any(data => data > DateTime.Now);
            bool hoje = workshopModelo.Datas.Any(data => data.Date == DateTime.Now.Date);
            bool datasPassadas = workshopModelo.Datas.Any(data => data < DateTime.Now);

            if (!datasFuturas && !hoje)
                return Enums.Status.Concluido.GetDescription();

            if ((datasPassadas && datasFuturas) || hoje)
                return Enums.Status.EmAndamento.GetDescription();

            if (!datasPassadas && !hoje && datasFuturas)
                return Enums.Status.Agendado.GetDescription();

            return workshopModelo.Status;
        }
    }
}

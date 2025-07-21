using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workshop.DB;
using Workshop.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InstrutorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstrutorController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]

        public ActionResult<IEnumerable<Instrutor>> Get()
        {
            var Instrutores = _context.Instrutor.
                OrderBy(c => c.Nome)
                .Select(instrutor => new
                {
                    instrutor.Nome,
                    instrutor.Cpf,
                    instrutor.Email,
                    instrutor.Login,
                    instrutor.Telefone,
                    instrutor.Perfil
                })
                .ToList();
            ;
            return Ok(Instrutores);
        }


        [HttpGet("{cpf}")]
        public ActionResult<Instrutor> Get(string cpf)
        {
            Instrutor? Instrutor = _context.Instrutor.Find(Instrutor.FormatCpf(cpf));
            if (Instrutor == null)
                return NotFound();

            var viewModel = new 
            {
                Instrutor.Nome,
                Instrutor.Cpf,
                Instrutor.Email,
                Instrutor.Login,
                Instrutor.Telefone,
                Instrutor.Perfil
            };

            return Ok(viewModel);

        }

        [HttpPost]
        public ActionResult Post([FromBody] Instrutor Instrutor)
        {
            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(Instrutor.FormatCpf(Instrutor.Cpf));
            if (InstrutorEncontrado != null)
                return BadRequest($"Um instrutor com este CPF já foi cadastrado.");

            InstrutorEncontrado = _context.Instrutor.Find(Instrutor.Login);
            if (InstrutorEncontrado != null)
                return BadRequest($"Um instrutor com este login já foi cadastrado.");

            Instrutor.Senha = HashPassword(Instrutor.Senha);
            _context.Instrutor.Add(Instrutor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { Instrutor.Cpf }, Instrutor);
        }

        [HttpPut("{cpf}")]
        public ActionResult Put(string cpf, [FromBody] Instrutor Instrutor)
        {

            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(Instrutor.FormatCpf(cpf));
            if (InstrutorEncontrado == null)
                return NotFound();

            if (Instrutor.Cpf == null)
                Instrutor.Cpf = InstrutorEncontrado.Cpf;

            Instrutor.Senha = HashPassword(Instrutor.Senha);
            _context.Entry(InstrutorEncontrado).State = EntityState.Detached;

            _context.Entry(Instrutor).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{cpf}")]
        public ActionResult Delete(string cpf)
        {

            Instrutor? Instrutor = _context.Instrutor.Find(Instrutor.FormatCpf(cpf));
            if (Instrutor == null)
                return NotFound();


            _context.Instrutor.Remove(Instrutor);
            _context.SaveChanges();
            return NoContent();

        }

        private string HashPassword(string senha)
        {
            var passwordHasher = new PasswordHasher<Instrutor>();
            return passwordHasher.HashPassword(null, senha);
        }

}
}

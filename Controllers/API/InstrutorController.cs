using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workshop.DB;
using Workshop.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workshop.Controllers.API
{
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
            List<Instrutor> Instrutores = _context.Instrutor.
                OrderBy(c => c.Nome)
                .ToList();
            ;
            return Ok(Instrutores);
        }


        [HttpGet("{cpf}")]
        public ActionResult<Instrutor> Get(string cpf)
        {
            Instrutor? Instrutor = _context.Instrutor.Find(cpf);
            if (Instrutor == null)
                return NotFound();

            return Ok(Instrutor);

        }

        [HttpPost]
        public ActionResult Post([FromBody] Instrutor Instrutor)
        {
            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(Instrutor.Cpf);
            if (InstrutorEncontrado != null)
                return BadRequest($"Colaborador já foi cadastrado: {InstrutorEncontrado.Nome}");

            _context.Instrutor.Add(Instrutor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { Instrutor.Cpf }, Instrutor);
        }

        [HttpPut("{cpf}")]
        public ActionResult Put(string cpf, [FromBody] Instrutor Instrutor)
        {

            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(cpf);
            if (InstrutorEncontrado == null)
                return NotFound();

            if (Instrutor.Cpf == null)
                Instrutor.Cpf = InstrutorEncontrado.Cpf;

            _context.Entry(InstrutorEncontrado).State = EntityState.Detached;

            _context.Entry(Instrutor).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{cpf}")]
        public ActionResult Delete(string cpf)
        {

            Instrutor? Instrutor = _context.Instrutor.Find(cpf);
            if (Instrutor == null)
                return NotFound();


            _context.Instrutor.Remove(Instrutor);
            _context.SaveChanges();
            return NoContent();

        }
    }
}

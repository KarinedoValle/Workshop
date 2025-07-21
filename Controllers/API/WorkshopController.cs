using Microsoft.AspNetCore.Authorization;
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
             .Include(w => w.Instrutor)
             .ToList();

            

            return Ok(Models.Workshop.Sort(workshops));

        }


        [HttpGet("{id}")]
        public ActionResult<Models.Workshop> Get(int id)
        {

            Models.Workshop? workshop = _context.Workshop
             .Include(w => w.Instrutor)
             .FirstOrDefault(w => w.ID == id);

            if (workshop == null)
                return NotFound();

            return Ok(workshop);

        }

        [HttpPost]
        public ActionResult Post([FromBody] WorkshopRequest workshop)
        {
            Instrutor? Instrutor = _context.Instrutor.FirstOrDefault(instrutor => instrutor.Cpf == Instrutor.FormatCpf(workshop.InstrutorCpf));
            if (Instrutor == null)
                return NotFound($"Instrutor com CPF {workshop.InstrutorCpf} não encontrado.");

            Models.Workshop workshopModelo = Models.Workshop.ConverteParaModelo(workshop, Instrutor);

            _context.Workshop.Add(workshopModelo);

             _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = workshopModelo.ID }, workshopModelo);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] WorkshopRequest workshop)
        {

            Models.Workshop? workshopEncontrado = _context.Workshop.Find(id);
            if (workshopEncontrado == null)
                return NotFound($"Workshop com Id {id} não encontrado.");

            Instrutor? Instrutor = _context.Instrutor.FirstOrDefault(instrutor => instrutor.Cpf == Instrutor.FormatCpf(workshop.InstrutorCpf));
            if (Instrutor == null)
                return NotFound($"Instrutor com CPF {workshop.InstrutorCpf} não encontrado.");

            Models.Workshop workshopModelo = Models.Workshop.ConverteParaModelo(workshop, Instrutor, workshopEncontrado.ID);

            _context.Entry(workshopEncontrado).State = EntityState.Detached;

            _context.Entry(workshopModelo).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            Models.Workshop? workshop = _context.Workshop.Find(id);
            if (workshop == null)
                return NotFound();


            _context.Workshop.Remove(workshop);
            _context.SaveChanges();
            return NoContent();

        }
    }
}

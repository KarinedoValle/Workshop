using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workshop.DB;
using Workshop.Models;
using System.Text.RegularExpressions;


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
            var user = HttpContext.User;
            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
                return new ObjectResult("Usuário sem permissão para cadastrar instrutores.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };


            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(Instrutor.FormatCpf(Instrutor.Cpf));
            if (InstrutorEncontrado != null)
                return BadRequest($"Um instrutor com este CPF já foi cadastrado.");

            string loginNormalizado = Instrutor.Login.Trim().ToUpper();

            bool loginCadastrado = _context.Instrutor
                .Any(i => i.Login.Trim().ToUpper() == loginNormalizado);
            if (loginCadastrado)
                return BadRequest($"Um instrutor com este login já foi cadastrado.");

            if(Regex.Replace(Instrutor.Cpf, @"[^\d]", "").Length != 11)
                return BadRequest($"CPF inválido. Este campo deve ter 11 caracteres.");

            if (Regex.Replace(Instrutor.Telefone, @"[^\d]", "").Length != 10 && Regex.Replace(Instrutor.Telefone, @"[^\d]", "").Length != 11)
                return BadRequest($"Telefone inválido. Este campo deve ter 10 ou 11 caracteres.");

            Instrutor.Senha = HashPassword(Instrutor.Senha);
            _context.Instrutor.Add(Instrutor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { Instrutor.Cpf }, Instrutor);
        }

        [HttpPut("{cpf}")]
        public ActionResult Put(string cpf, [FromBody] Instrutor Instrutor)
        {
            var user = HttpContext.User;
            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
                return new ObjectResult("Usuário sem permissão para editar instrutores.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            string cpfFormatado = Instrutor.FormatCpf(cpf);
            Instrutor? InstrutorEncontrado = _context.Instrutor.Find(cpfFormatado);
            if (InstrutorEncontrado == null)
                return BadRequest($"Não foi encontrado instrutor com este CPF.");

            string loginNormalizado = Instrutor.Login.Trim().ToUpper();

            bool loginCadastrado = _context.Instrutor
                .Any(i => i.Login.Trim().ToUpper() == loginNormalizado);
            bool alteracaoLogin = !Instrutor.Login.Trim().Equals(InstrutorEncontrado.Login.Trim(), StringComparison.OrdinalIgnoreCase);
            if (alteracaoLogin && loginCadastrado)
                return BadRequest($"Um instrutor com este login já foi cadastrado.");

            if (Instrutor.Cpf == null)
                Instrutor.Cpf = cpfFormatado;

            Instrutor.Senha = HashPassword(Instrutor.Senha);
            _context.Entry(InstrutorEncontrado).State = EntityState.Detached;

            _context.Entry(Instrutor).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{cpf}")]
        public ActionResult Delete(string cpf)
        {
            var user = HttpContext.User;
            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
                return new ObjectResult("Usuário sem permissão para deletar instrutores.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

            Instrutor? Instrutor = _context.Instrutor.Find(Instrutor.FormatCpf(cpf));
            if (Instrutor == null)
                return NotFound();

            Models.Workshop? workshop = _context.Workshop.Include(w => w.Instrutor).FirstOrDefault(w => w.Instrutor.Cpf == Instrutor.FormatCpf(cpf));
            if (workshop != null)
                return BadRequest($"Não é possível deletar instrutores com workshops associados.");

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

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Workshop.DB;
//using Workshop.Models;
//using System.Text.RegularExpressions;


//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Workshop.Controllers.API
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsuarioController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public UsuarioController(AppDbContext context)
//        {
//            _context = context;
//        }


//        [HttpGet]

//        public ActionResult<IEnumerable<Usuario>> Get()
//        {
//            var user = HttpContext.User;
//            bool admin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
            
//            var Usuarios = _context.Usuario.
//                OrderBy(c => c.Nome)
//                .Select(Usuario => new
//                {
//                    Usuario.Nome,
//                    Usuario.Cpf,
//                    Usuario.Email,
//                    Usuario.Login,
//                    Usuario.Telefone,
//                    Usuario.Perfil
//                })
//                .ToList();

//            if (admin)
//                return Ok(Usuarios);

//            return Ok(Usuarios.FindAll(usuario => usuario.Cpf == User.FindFirst("Cpf")?.Value));
//        }


//        [HttpGet("{cpf}")]
//        public ActionResult<Usuario> Get(string cpf)
//        {
//            Usuario? Usuario = _context.Usuario.Find(Usuario.FormatCpf(cpf));
//            if (Usuario == null)
//                return NotFound();

//            var user = HttpContext.User;
//            bool admin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());

//            var viewModel = new 
//            {
//                Usuario.Nome,
//                Usuario.Cpf,
//                Usuario.Email,
//                Usuario.Login,
//                Usuario.Telefone,
//                Usuario.Perfil
//            };

//            if (admin)
//                return Ok(viewModel);

//            return new ObjectResult("Sem permissão para visualizar este usuário.")
//            {
//                StatusCode = StatusCodes.Status403Forbidden
//            };

//        }

//        [HttpPost]
//        public ActionResult Post([FromBody] Usuario Usuario)
//        {
//            var user = HttpContext.User;
//            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
//                return new ObjectResult("Sem permissão para cadastrar usuários.")
//                {
//                    StatusCode = StatusCodes.Status403Forbidden
//                };


//            Usuario? UsuarioEncontrado = _context.Usuario.Find(Usuario.FormatCpf(Usuario.Cpf));
//            if (UsuarioEncontrado != null)
//                return BadRequest($"Um Usuario com este CPF já foi cadastrado.");

//            string loginNormalizado = Usuario.Login.Trim().ToUpper();

//            bool loginCadastrado = _context.Usuario
//                .Any(i => i.Login.Trim().ToUpper() == loginNormalizado);
//            if (loginCadastrado)
//                return BadRequest($"Um Usuario com este login já foi cadastrado.");

//            if(Regex.Replace(Usuario.Cpf, @"[^\d]", "").Length != 11)
//                return BadRequest($"CPF inválido. Este campo deve ter 11 caracteres.");

//            if (Regex.Replace(Usuario.Telefone, @"[^\d]", "").Length != 10 && Regex.Replace(Usuario.Telefone, @"[^\d]", "").Length != 11)
//                return BadRequest($"Telefone inválido. Este campo deve ter 10 ou 11 caracteres.");

//            Usuario.Senha = HashPassword(Usuario.Senha);
//            _context.Usuario.Add(Usuario);
//            _context.SaveChanges();
//            return CreatedAtAction(nameof(Get), new { Usuario.Cpf }, Usuario);
//        }

//        [HttpPut("{cpf}")]
//        public ActionResult Put(string cpf, [FromBody] Usuario Usuario)
//        {
//            string cpfFormatado = Usuario.FormatCpf(cpf);
//            Usuario? UsuarioEncontrado = _context.Usuario.Find(cpfFormatado);
//            if (UsuarioEncontrado == null)
//                return BadRequest($"Não foi encontrado usuário com este CPF.");


//            var user = HttpContext.User;
//            bool isAdmin = user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription());
//            bool proprioCPF = User.FindFirst("Cpf")?.Value == cpfFormatado;
//            if (!proprioCPF && !isAdmin)
//                return new ObjectResult("Sem permissão para editar usuários.")
//                {
//                    StatusCode = StatusCodes.Status403Forbidden
//                };


//            string loginNormalizado = Usuario.Login.Trim().ToUpper();
//            bool loginCadastrado = _context.Usuario
//                .Any(i => i.Login.Trim().ToUpper() == loginNormalizado);
//            bool alteracaoLogin = !Usuario.Login.Trim().Equals(UsuarioEncontrado.Login.Trim(), StringComparison.OrdinalIgnoreCase);
//            if (alteracaoLogin && loginCadastrado)
//                return BadRequest($"Um usuário com este login já foi cadastrado.");


//            bool cpfAlterado = Usuario.Cpf != null && Usuario.Cpf != UsuarioEncontrado.Cpf;
//            bool perfilAlterado = Usuario.Perfil != User.FindFirst("Perfil")?.Value;

//            if(cpfAlterado)
//                return BadRequest($"Não é possível alterar CPF.");

//            if (perfilAlterado && !isAdmin)
//                return BadRequest($"Usuário sem permissão para alterar perfil.");

//            if (Usuario.Cpf == null)
//                Usuario.Cpf = cpfFormatado;

//            bool senhaAlterada = String.IsNullOrEmpty(Usuario.Senha.Trim());
//            Usuario.Senha = senhaAlterada ? HashPassword(Usuario.Senha) : UsuarioEncontrado.Senha;
            
//            _context.Entry(UsuarioEncontrado).State = EntityState.Detached;

//            _context.Entry(Usuario).State = EntityState.Modified;
//            _context.SaveChanges();
//            return NoContent();

//        }

//        [HttpDelete("{cpf}")]
//        public ActionResult Delete(string cpf)
//        {
//            var user = HttpContext.User;
//            if (!user.HasClaim(c => c.Type == "Perfil" && c.Value == Enums.Perfil.Administrador.GetDescription()))
//                return new ObjectResult("Sem permissão para deletar usuários.")
//                {
//                    StatusCode = StatusCodes.Status403Forbidden
//                };

//            Usuario? Usuario = _context.Usuario.Find(Usuario.FormatCpf(cpf));
//            if (Usuario == null)
//                return NotFound();

//            Models.Workshop? workshop = _context.Workshop.Include(w => w.Usuario).FirstOrDefault(w => w.Usuario.Cpf == Usuario.FormatCpf(cpf));
//            if (workshop != null)
//                return BadRequest($"Não é possível deletar usuários com workshops associados.");

//            _context.Usuario.Remove(Usuario);
//            _context.SaveChanges();
//            return NoContent();

//        }

//        private string HashPassword(string senha)
//        {
//            var passwordHasher = new PasswordHasher<Usuario>();
//            return passwordHasher.HashPassword(null, senha);
//        }

//}
//}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Workshop.DB;

namespace Workshop.Services.API.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<object> ObterTodos(bool isAdmin, string? cpfUsuarioLogado)
        {
            var usuarios = _context.Usuario
                .OrderBy(u => u.Nome)
                .Select(u => new
                {
                    u.Nome,
                    u.Cpf,
                    u.Email,
                    u.Login,
                    u.Telefone,
                    u.Perfil
                })
                .ToList();

            return isAdmin
                ? usuarios
                : usuarios.Where(u => u.Cpf == cpfUsuarioLogado);
        }

        public (bool sucesso, object? resultado, string? erro) ObterPorCpf(string cpf, string cpfUsuario, bool isAdmin)
        {
            string cpfFormatado = Models.Usuario.FormatCpf(cpf);
            string cpfUsuarioLogado = Models.Usuario.FormatCpf(cpfUsuario);
            bool proprioCPF = cpfUsuarioLogado == cpfFormatado;
            if (!proprioCPF && !isAdmin)
                return (false, null, "Sem permissão para visualizar este usuário.");

            var usuario = _context.Usuario.Find(Models.Usuario.FormatCpf(cpf));
            if (usuario == null)
                return (false, null, "Usuário não encontrado.");

            var viewModel = new
            {
                usuario.Nome,
                usuario.Cpf,
                usuario.Email,
                usuario.Login,
                usuario.Telefone,
                usuario.Perfil
            };

            

            return (true, viewModel, null);
        }

        public (bool sucesso, string? erro, Models.Usuario? usuario) Criar(Models.Usuario usuario, bool isAdmin)
        {
            if (!isAdmin)
                return (false, "Sem permissão para cadastrar usuários.", null);

            if (_context.Usuario.Find(Models.Usuario.FormatCpf(usuario.Cpf)) != null)
                return (false, "Um usuário com este CPF já foi cadastrado.", null);

            string loginNormalizado = usuario.Login.Trim().ToUpper();
            if (_context.Usuario.Any(u => u.Login.Trim().ToUpper() == loginNormalizado))
                return (false, "Um usuário com este login já foi cadastrado.", null);

            if (Regex.Replace(usuario.Cpf, @"[^\d]", "").Length != 11)
                return (false, "CPF inválido. Este campo deve ter 11 caracteres.", null);

            int telefoneLength = Regex.Replace(usuario.Telefone, @"[^\d]", "").Length;
            if (telefoneLength < 10 || telefoneLength > 11)
                return (false, "Telefone inválido. Este campo deve ter 10 ou 11 caracteres.", null);

            usuario.Senha = HashPassword(usuario.Senha);
            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return (true, null, usuario);
        }

        public (bool sucesso, string? erro) Atualizar(string cpf, Models.Usuario usuario, bool isAdmin, string? cpfUsuarioLogado, string? perfilUsuario)
        {
            string cpfFormatado = Models.Usuario.FormatCpf(cpf);
            bool proprioCPF = cpfUsuarioLogado == cpfFormatado;
            if (!proprioCPF && !isAdmin)
                return (false, "Sem permissão para editar usuário.");

            var existente = _context.Usuario.Find(cpfFormatado);
            if (existente == null)
                return (false, "Usuário não encontrado.");

            string loginNormalizado = usuario.Login.Trim().ToUpper();
            bool loginCadastrado = _context.Usuario
                .Any(u => u.Login.Trim().ToUpper() == loginNormalizado && u.Cpf != cpfFormatado);

            if (loginCadastrado)
                return (false, "Um usuário com este login já foi cadastrado.");

            if (usuario.Cpf != existente.Cpf)
                return (false, "Não é possível alterar o CPF.");

            bool perfilAlterado = usuario.Perfil != perfilUsuario;
            if (perfilAlterado && !isAdmin)
                return (false, "Usuário sem permissão para alterar o perfil.");

            bool senhaFoiInformada = !string.IsNullOrWhiteSpace(usuario.Senha);
            bool senhaAlterada = usuario.Senha != existente.Senha;
            usuario.Senha = senhaFoiInformada && senhaAlterada ? HashPassword(usuario.Senha) : existente.Senha;

            _context.Entry(existente).State = EntityState.Detached;
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();

            return (true, null);
        }

        public (bool sucesso, string? erro) Deletar(string cpf, bool isAdmin)
        {
            if (!isAdmin)
                return (false, "Sem permissão para deletar usuários.");

            var usuario = _context.Usuario.Find(Models.Usuario.FormatCpf(cpf));
            if (usuario == null)
                return (false, null);

            bool possuiWorkshop = _context.Workshop.Include(w => w.Usuario)
                .Any(w => w.Usuario.Cpf == usuario.Cpf);

            if (possuiWorkshop)
                return (false, "Não é possível deletar usuários com workshops associados.");

            _context.Usuario.Remove(usuario);
            _context.SaveChanges();

            return (true, null);
        }

        private string HashPassword(string senha)
        {
            var passwordHasher = new PasswordHasher<Models.Usuario>();
            return passwordHasher.HashPassword(null!, senha);
        }
    }
}
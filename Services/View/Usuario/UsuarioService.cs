using Workshop.DB;
using Workshop.Enums;

namespace Workshop.Services.View.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public bool UsuarioTemPermissao(string usuarioCpf, string cpfRequerido)
        {
            return usuarioCpf == cpfRequerido;
        }

        public List<string> ObterPerfis()
        {
            return EnumExtensions.GetEnumDescriptions<Perfil>().ToList();
        }

        public Models.Usuario? ObterUsuarioPorCpf(string cpf)
        {
            return _context.Usuario.FirstOrDefault(u => u.Cpf == cpf);
        }
    }
}
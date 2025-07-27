using Microsoft.EntityFrameworkCore;
using Workshop.DB;
using Workshop.Enums;
using Workshop.Models;

public class CadastroService : ICadastroService
{
    private readonly AppDbContext _context;

    public CadastroService(AppDbContext context)
    {
        _context = context;
    }

    public bool UsuarioTemPermissao(string usuarioCpf, Perfil perfilRequerido)
    {
        var perfilUsuario = _context.Usuario
            .FirstOrDefault(u => u.Cpf == usuarioCpf)?.Perfil;

        return perfilUsuario == perfilRequerido.GetDescription();
    }

    public List<Usuario> ObterUsuarios()
    {
        return _context.Usuario.ToList();
    }

    public List<string> ObterCategorias()
    {
        return EnumExtensions.GetEnumDescriptions<Categoria>().ToList();
    }

    public List<string> ObterModalidades()
    {
        return EnumExtensions.GetEnumDescriptions<Modalidade>().ToList();
    }

    public List<string> ObterStatus()
    {
        return EnumExtensions.GetEnumDescriptions<Status>().ToList();
    }

    public Workshop.Models.Workshop? ObterWorkshop(int id)
    {
        return _context.Workshop
            .Include(w => w.Usuario)
            .FirstOrDefault(w => w.ID == id);
    }
}

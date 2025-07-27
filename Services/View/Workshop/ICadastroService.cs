using Workshop.Models;
using Workshop.Enums;

public interface ICadastroService
{
    bool UsuarioTemPermissao(string usuarioCpf, Perfil perfilRequerido);
    List<Usuario> ObterUsuarios();
    List<string> ObterCategorias();
    List<string> ObterModalidades();
    List<string> ObterStatus();
    Workshop.Models.Workshop? ObterWorkshop(int id);
}

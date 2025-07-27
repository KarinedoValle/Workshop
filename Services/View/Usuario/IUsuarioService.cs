namespace Workshop.Services.View.Usuario
{
    public interface IUsuarioService
    {
        bool UsuarioTemPermissao(string usuarioCpf, string cpfRequerido);
        List<string> ObterPerfis();
        Models.Usuario? ObterUsuarioPorCpf(string cpf);
    }
}
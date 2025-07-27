namespace Workshop.Services.API.Usuario
{
    public interface IUsuarioService
    {
        IEnumerable<object> ObterTodos(bool isAdmin, string? cpf);
        (bool sucesso, object? resultado, string? erro) ObterPorCpf(string cpf, string cpfUsuario, bool isAdmin);
        (bool sucesso, string? erro, Models.Usuario? usuario) Criar(Models.Usuario usuario, bool isAdmin);
        (bool sucesso, string? erro) Atualizar(string cpf, Models.Usuario usuario, bool isAdmin, string? cpfUsuarioLogado, string? perfilUsuario);
        (bool sucesso, string? erro) Deletar(string cpf, bool isAdmin);
    }
}
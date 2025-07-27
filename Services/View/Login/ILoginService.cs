public interface ILoginService
{
    (bool sucesso, string? token, string? mensagemErro) Autenticar(string username, string senha);
}

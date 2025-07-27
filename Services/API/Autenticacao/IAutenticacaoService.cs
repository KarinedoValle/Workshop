using Workshop.Models;

public interface IAutenticacaoService
{
    (bool sucesso, object? resultado, string? erro) GerarToken(Token request);
}

using Workshop.Models;

public interface IWorkshopService
{
    IEnumerable<Workshop.Models.WorkshopResponse> ObterTodos();
    Workshop.Models.WorkshopResponse? ObterPorId(int id, string? cpfUsuario, bool isAdmin, out string? erroPermissao);
    (bool sucesso, string? erro, Workshop.Models.WorkshopResponse? workshopCriado) Criar(WorkshopRequest request, string cpfUsuario, bool isAdmin);
    (bool sucesso, string? erro) Atualizar(int id, WorkshopRequest request, string cpfUsuario, bool isAdmin);
    (bool sucesso, string? erro) Deletar(int id, bool isAdmin);
}

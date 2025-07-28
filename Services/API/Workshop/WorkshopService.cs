using Microsoft.EntityFrameworkCore;
using System.Linq;
using Workshop.DB;
using Workshop.Enums;
using Workshop.Models;

namespace Workshop.Services.API.Workshop
{
    public class WorkshopService : IWorkshopService
    {
        private readonly AppDbContext _context;

        public WorkshopService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.WorkshopResponse> ObterTodos()
        {
            var workshops = _context.Workshop.Include(w => w.Usuario).ToList();
            var responses = workshops.Select(workshop => new WorkshopResponse(
                workshop.ID,
                workshop.Nome,
                workshop.Descricao,
                workshop.Datas,
                workshop.Usuario.Nome,
                workshop.Categoria,
                workshop.Modalidade,
                workshop.Status
            )).ToList();
            return Models.Workshop.Sort(responses);
        }

        public Models.WorkshopResponse? ObterPorId(int id, string? cpfUsuario, bool isAdmin, out string? erroPermissao)
        {
            erroPermissao = null;

            var workshop = _context.Workshop.Include(w => w.Usuario).FirstOrDefault(w => w.ID == id);
            if (workshop == null) {
                erroPermissao = "Workshop não encontrado.";
                return null;
            }

            bool proprioCPF = cpfUsuario == Models.Usuario.FormatCpf(workshop.Usuario.Cpf);
            if (!proprioCPF && !isAdmin)
            {
                erroPermissao = "Sem permissão para visualizar este workshop.";
                return null;
            }

            return new WorkshopResponse(workshop.ID, 
                workshop.Nome, 
                workshop.Descricao, 
                workshop.Datas, 
                workshop.Usuario.Nome, 
                workshop.Categoria, 
                workshop.Modalidade,
                workshop.Status);
        }

        public (bool sucesso, string? erro, Models.WorkshopResponse? workshopCriado) Criar(WorkshopRequest request, string cpfUsuario, bool isAdmin)
        {
            if (!isAdmin)
                return (false, "Sem permissão para cadastrar workshops.", null);

            var usuario = _context.Usuario.FirstOrDefault(u => u.Cpf == Models.Usuario.FormatCpf(request.UsuarioCpf));
            if (usuario == null)
                return (false, $"Usuário com CPF {request.UsuarioCpf} não encontrado.", null);

            if (usuario.Perfil == Perfil.Leitor.GetDescription())
                return (false, "Não é possível cadastrar workshops com um instrutor que tenha perfil 'Leitor'.", null);

            var workshop = Models.Workshop.ConverteParaModelo(request, usuario);

            if (!ValidarHorarioComercial(workshop.Datas))
                return (false, "Encontros fora do horário comercial (8h às 19h) não são permitidos.", null);

            workshop.Status = AtualizarStatus(workshop);

            _context.Workshop.Add(workshop);
            _context.SaveChanges();

            WorkshopResponse responde = new (workshop.ID,
                workshop.Nome,
                workshop.Descricao,
                workshop.Datas,
                workshop.Usuario.Nome,
                workshop.Categoria,
                workshop.Modalidade,
                workshop.Status);
            return (true, null, responde);
        }

        public (bool sucesso, string? erro) Atualizar(int id, WorkshopRequest request, string cpfUsuario, bool isAdmin)
        {
            var existente = _context.Workshop.Include(w => w.Usuario).FirstOrDefault(w => w.ID == id);
            if (existente == null)
                return (false, $"Workshop com Id {id} não encontrado.");

            if (existente.Status == Status.Concluido.GetDescription())
                return (false, "Não é possível editar workshops com status concluído.");

            bool proprioCPF = cpfUsuario == Models.Usuario.FormatCpf(request.UsuarioCpf);
            bool instrutorAlterado = existente.Usuario.Cpf != Models.Usuario.FormatCpf(request.UsuarioCpf);

            if (!proprioCPF && !isAdmin)
                return (false, "Usuário sem permissão para editar este workshop.");

            if (proprioCPF && instrutorAlterado && !isAdmin)
                return (false, "Usuário sem permissão para alterar o instrutor.");

            var usuario = _context.Usuario.FirstOrDefault(u => u.Cpf == Models.Usuario.FormatCpf(request.UsuarioCpf));
            if (usuario == null)
                return (false, $"Usuário com CPF {request.UsuarioCpf} não encontrado.");

            var workshop = Models.Workshop.ConverteParaModelo(request, usuario, existente.ID);
            if (!ValidarHorarioComercial(workshop.Datas))
                return (false, "Encontros fora do horário comercial (8h às 19h) não são permitidos.");

            workshop.Status = AtualizarStatus(workshop);

            _context.Entry(existente).State = EntityState.Detached;
            _context.Entry(workshop).State = EntityState.Modified;
            _context.SaveChanges();

            return (true, null);
        }

        public (bool sucesso, string? erro) Deletar(int id, bool isAdmin)
        {
            if (!isAdmin)
                return (false, "Usuário sem permissão para deletar workshops.");

            var workshop = _context.Workshop.Find(id);
            if (workshop == null)
                return (false, "Workshop não encontrado.");

            _context.Workshop.Remove(workshop);
            _context.SaveChanges();
            return (true, null);
        }

        private static bool ValidarHorarioComercial(List<DateTime>? datas)
        {
            if (datas == null || datas.Count == 0)
                return true;

            return !datas.Any(data => data.TimeOfDay < TimeSpan.FromHours(8) || data.TimeOfDay > TimeSpan.FromHours(19));
        }

        private static string AtualizarStatus(Models.Workshop w)
        {
            if (w.Datas.Count == 0)
                return Status.Novo.GetDescription();

            bool futuras = w.Datas.Any(d => d > DateTime.Now);
            bool hoje = w.Datas.Any(d => d.Date == DateTime.Now.Date);
            bool passadas = w.Datas.Any(d => d < DateTime.Now);

            if (!futuras && !hoje)
                return Status.Concluido.GetDescription();
            if (passadas && futuras || hoje)
                return Status.EmAndamento.GetDescription();
            if (!passadas && futuras)
                return Status.Agendado.GetDescription();

            return w.Status;
        }
    }
}
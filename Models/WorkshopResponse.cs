namespace Workshop.Models
{
    public class WorkshopResponse
    {
        public int ID { get; private set; }
        public string Nome { get; private set; }

        public string Descricao { get; private set; }

        public List<DateTime> Datas { get; private set; }

        public string Instrutor { get; private set; }

        public string Categoria { get; private set; }

        public string Modalidade { get; private set; }

        public string Status { get; private set; }

        public WorkshopResponse(int id, string nome, string descricao, List<DateTime> datas, string instrutor, string categoria, string modalidade, string status)
        {
            ID = id;
            Nome = nome;
            Descricao = descricao;
            Datas = datas;
            Instrutor = instrutor;
            Categoria = categoria;
            Modalidade = modalidade;
            Status = status;
        }
    }
}

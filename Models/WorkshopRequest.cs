namespace Workshop.Models
{
    public class WorkshopRequest
    {

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<DateTime> Datas { get; set; }
        public string InstrutorCpf { get; set; }
        public string Categoria { get; set; }
        public string Modalidade { get; set; }
        public string Status { get; set; }

    }
}

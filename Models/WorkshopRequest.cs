namespace Workshop.Models
{
    public class WorkshopRequest
    {

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<DateTime> Datas { get; set; }
        public InstrutorCpf Instrutor { get; set; }
        public string CategoriaTexto { get; set; }
        public string ModalidadeTexto { get; set; }
        public string StatusTexto { get; set; }

    }
}

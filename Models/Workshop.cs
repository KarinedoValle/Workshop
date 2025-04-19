using Workshop.Enums;

namespace Workshop.Models
{
    public class Workshop
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public Categoria Categoria { get; set; }

        public Modalidade Modalidade { get; set; }

        public string Descricao { get; set; }

        public List<DateTime> Datas { get; set; }
    }
}

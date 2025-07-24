using System.ComponentModel.DataAnnotations;

namespace Workshop.Models
{
    public class WorkshopRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatório.")]
        public string Descricao { get; set; }

        public List<DateTime> Datas { get; set; }

       [Required(ErrorMessage = "O instrutor é obrigatório.")]
        public string InstrutorCpf { get; set; }

       [Required(ErrorMessage = "A categoria é obrigatório.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "A modalidade é obrigatório.")]
        public string Modalidade { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; }

    }
}

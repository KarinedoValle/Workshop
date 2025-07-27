using System.ComponentModel.DataAnnotations;

namespace Workshop.Models
{
    public class WorkshopRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatório.")]
        public string Descricao { get; set; }

        public List<DateTimeOffset> Datas { get; set; }

       [Required(ErrorMessage = "O Usuario é obrigatório.")]
        public string UsuarioCpf { get; set; }

       [Required(ErrorMessage = "A categoria é obrigatório.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "A modalidade é obrigatório.")]
        public string Modalidade { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workshop.Enums;

namespace Workshop.Models
{
    public class WorkshopRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }

        [Column(TypeName = "timestamp without time zone[]")]
        public List<DateTime> Datas { get; set; }

       [Required(ErrorMessage = "O Usuario é obrigatório.")]
        public string UsuarioCpf { get; set; }

        [EnumValido(typeof(Categoria), ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; }

        [EnumValido(typeof(Modalidade), ErrorMessage = "A modalidade é obrigatória.")]
        public string Modalidade { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workshop.Enums;

namespace Workshop.Models
{
    public class WorkshopRequest
    {
        [Required(ErrorMessage = "Nome inválido.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Descrição inválida.")]
        public string Descricao { get; set; }

        [Column(TypeName = "timestamp without time zone[]")]
        public List<DateTime> Datas { get; set; }

       [Required(ErrorMessage = "Instrutor inválido.")]
        public string UsuarioCpf { get; set; }

        [EnumValido(typeof(Categoria), ErrorMessage = "Categoria inválida.")]
        public string Categoria { get; set; }

        [EnumValido(typeof(Modalidade), ErrorMessage = "Modalidade inválida.")]
        public string Modalidade { get; set; }

    }
}

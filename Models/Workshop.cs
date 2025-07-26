using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Workshop.Enums;
using Newtonsoft.Json;

namespace Workshop.Models
{
    public class Workshop
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        private Categoria? CategoriaEnum { get; set; }

        [NotMapped]
        private Modalidade? ModalidadeEnum { get; set; }

        [NotMapped]
        private Status? StatusEnum { get; set; }

        [NotMapped]
        private List<DateTime> _datas;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O instrutor é obrigatório.")]
        public Usuario Usuario { get; set; }

        public List<DateTime> Datas
        {
            get
            {
                return _datas?.Select(d => d.ToLocalTime()).ToList();
            }
            set
            {
                _datas = value?.Select(d => d.ToUniversalTime()).ToList();
            }
        }

        

        [Column("Categoria")]
        [JsonProperty("Categoria")]
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [EnumValido(typeof(Categoria), ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria
        {
            get => CategoriaEnum?.GetDescription() ?? string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    CategoriaEnum = null;
                }
                else
                {
                    try
                    {
                        CategoriaEnum = EnumExtensions.GetEnumByDescription<Categoria>(value);
                    }
                    catch (ArgumentException)
                    {
                        CategoriaEnum = null;
                    }
                }
            }
        }

        [JsonProperty("Modalidade")]
        [Column("Modalidade")]
        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        [EnumValido(typeof(Modalidade), ErrorMessage = "A modalidade é obrigatória.")]
        public string Modalidade
        {
            get => ModalidadeEnum?.GetDescription() ?? string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ModalidadeEnum = null;
                }
                else
                {
                    try
                    {
                        ModalidadeEnum = EnumExtensions.GetEnumByDescription<Modalidade>(value);
                    }
                    catch (ArgumentException)
                    {
                        ModalidadeEnum = null;
                    }
                }
            }
        }

        [Column("Status")]
        [JsonProperty("Status")]
        public string Status
        {
            get => StatusEnum?.GetDescription() ?? string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    StatusEnum = null;
                }
                else
                {
                    try
                    {
                        StatusEnum = EnumExtensions.GetEnumByDescription<Status>(value);
                    }
                    catch (ArgumentException)
                    {
                        StatusEnum = null;
                    }
                }
            }
        }

        public static Workshop ConverteParaModelo(WorkshopRequest request, Usuario Usuario, int? id = null) {
            if (id.HasValue) {
                return new Workshop
                {
                    ID = id.Value,
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    Datas = request.Datas,
                    Usuario = Usuario,
                    Categoria = request.Categoria,
                    Modalidade = request.Modalidade
                }; 
            }
            return new Workshop
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Datas = request.Datas,
                Usuario = Usuario,
                Categoria = request.Categoria,
                Modalidade = request.Modalidade
            };
        }

        public List<DateTime> AjustarDatasParaFusoHorarioLocal(List<DateTime> Datas)
        {
            if (Datas != null)
            {
                for (int i = 0; i < Datas.Count; i++)
                {
                    Datas[i] = Datas[i].ToLocalTime();
                }
            }
            return Datas ?? [];
        }


        public static List<Workshop> Sort(List<Workshop> workshops)
        {
            return workshops.OrderBy(w => w.ID).ToList();
        }


    }
}

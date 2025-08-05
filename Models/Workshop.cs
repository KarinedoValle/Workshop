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

        [Required(ErrorMessage = "Nome inválido.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Descrição inválida.")]
        public string Descricao { get; set; }

        public Usuario Usuario { get; set; }

        [Column(TypeName = "timestamp without time zone[]")]
        public List<DateTime> Datas
        {
            get
            {
                return _datas?.Select(d => DateTime.SpecifyKind(d, DateTimeKind.Local)).ToList(); ;
            }
            set
            {
                _datas = value;
            }
        }



        [Column("Categoria")]
        [JsonProperty("Categoria")]
        [EnumValido(typeof(Categoria), ErrorMessage = "Categoria inválida.")]
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
        [EnumValido(typeof(Modalidade), ErrorMessage = "Modalidade inválida.")]
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


        public static List<WorkshopResponse> Sort(List<WorkshopResponse> workshops)
        {
            return workshops.OrderBy(w => w.ID).ToList();
        }
    }
}

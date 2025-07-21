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
        private Categoria CategoriaEnum { get; set; }

        [NotMapped]
        private Modalidade ModalidadeEnum { get; set; }

        [NotMapped]
        private Status StatusEnum { get; set; }

        [NotMapped]
        private List<DateTime> _datas;

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public Instrutor Instrutor { get; set; }

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
        public string Categoria
        {
            get { return CategoriaEnum.GetDescription(); }
            set { CategoriaEnum = EnumExtensions.GetEnumByDescription<Categoria>(value); }
        }

        [JsonProperty("Modalidade")]
        [Column("Modalidade")]
        public string Modalidade
        {
            get { return ModalidadeEnum.GetDescription(); }
            set { ModalidadeEnum = EnumExtensions.GetEnumByDescription<Modalidade>(value); }
        }

        [Column("Status")]
        [JsonProperty("Status")]
        public string Status
        {
            get { return StatusEnum.GetDescription(); }
            set { StatusEnum = EnumExtensions.GetEnumByDescription<Status>(value); }
        }

        public static Workshop ConverteParaModelo(WorkshopRequest request, Instrutor Instrutor, int? id = null) {
            if (id.HasValue) {
                return new Workshop
                {
                    ID = id.Value,
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    Datas = request.Datas,
                    Instrutor = Instrutor,
                    Categoria = request.Categoria,
                    Modalidade = request.Modalidade,
                    Status = request.Status
                }; 
            }
            return new Workshop
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Datas = request.Datas,
                Instrutor = Instrutor,
                Categoria = request.Categoria,
                Modalidade = request.Modalidade,
                Status = request.Status
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

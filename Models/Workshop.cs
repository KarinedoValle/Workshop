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

        public string Nome { get; set; }

        [NotMapped]
        private Categoria Categoria { get; set; }

        [NotMapped]
        private Modalidade Modalidade { get; set; }

        public string Descricao { get; set; }


        private List<DateTime> _datas;

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


        public Instrutor Instrutor { get; set; }

        [NotMapped]
        private Status Status { get; set; }

        [Column("Categoria")]
        [JsonProperty("Categoria")]
        public string CategoriaTexto
        {
            get { return Categoria.GetDescription(); }
            set { Categoria = EnumExtensions.GetEnumByDescription<Categoria>(value); }
        }

        [JsonProperty("Modalidade")]
        [Column("Modalidade")]
        public string ModalidadeTexto
        {
            get { return Modalidade.GetDescription(); }
            set { Modalidade = EnumExtensions.GetEnumByDescription<Modalidade>(value); }
        }

        [JsonProperty("Status")]
        [Column("Status")]
        public string StatusTexto
        {
            get { return Status.GetDescription(); }
            set { Status = EnumExtensions.GetEnumByDescription<Status>(value); }
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
                    CategoriaTexto = request.CategoriaTexto,
                    ModalidadeTexto = request.ModalidadeTexto,
                    StatusTexto = request.StatusTexto
                }; 
            }
            return new Workshop
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Datas = request.Datas,
                Instrutor = Instrutor,
                CategoriaTexto = request.CategoriaTexto,
                ModalidadeTexto = request.ModalidadeTexto,
                StatusTexto = request.StatusTexto
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

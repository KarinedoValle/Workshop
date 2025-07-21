using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Workshop.Enums;

namespace Workshop.Models
{
    public class Instrutor
    {
        [Key]
        public string Cpf
        {
            get => _cpf;
            set => _cpf = FormatCpf(value);
        }

        [NotMapped]
        private Perfil PerfilEnum { get; set; }

        [NotMapped]
        private string _cpf;

        [NotMapped]
        private string _telefone;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string Telefone
        {
            get => _telefone;
            set => _telefone = FormatTelefone(value);
        }
        
       
        [Column("Perfil")]
        [JsonProperty(nameof(Perfil))]
        public string Perfil
        {
            get { return PerfilEnum.GetDescription(); }
            set { PerfilEnum = EnumExtensions.GetEnumByDescription<Perfil>(value); }
        }

        public static string FormatCpf(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            cpf = cpf.PadLeft(11, '0');

            if (cpf.Length == 11)
                return $"{cpf[..3]}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
            

            return cpf;
        }
       

        private static string FormatTelefone(string telefone)
        {
            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            if (telefone.Length == 10)
                return $"({telefone[..2]}) {telefone.Substring(2, 4)}-{telefone.Substring(6, 4)}";
            else if (telefone.Length == 11)
                return $"({telefone[..2]}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
            

            return telefone;
        }



    }
}

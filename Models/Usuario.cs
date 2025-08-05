using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Workshop.Enums;

namespace Workshop.Models
{
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage = "CPF inválido.")]
        public string Cpf
        {
            get => _cpf;
            set => _cpf = FormatCpf(value);
        }

        [NotMapped]
        private Perfil? PerfilEnum { get; set; }

        [NotMapped]
        private string _cpf;

        [NotMapped]
        private string _telefone;

        [Required(ErrorMessage = "Senha inválida.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Nome inválido.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login inválido.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Telefone inválido.")]
        public string Telefone
        {
            get => _telefone;
            set => _telefone = FormatTelefone(value);
        }
        
       
        [Column("Perfil")]
        [JsonProperty(nameof(Perfil))]
        [EnumValido(typeof(Perfil), ErrorMessage = "Perfil inválido.")]
        public string Perfil
        {
            get => PerfilEnum?.GetDescription() ?? string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PerfilEnum = null;
                }
                else
                {
                    try
                    {
                        PerfilEnum = EnumExtensions.GetEnumByDescription<Perfil>(value);
                    }
                    catch (ArgumentException)
                    {
                        PerfilEnum = null;
                    }
                }
            }
        }

        public static string FormatCpf(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length == 11)
                return $"{cpf[..3]}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
            else
                return null;
        }
       

        private static string FormatTelefone(string telefone)
        {
            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            if (telefone.Length == 10)
                return $"({telefone[..2]}) {telefone.Substring(2, 4)}-{telefone.Substring(6, 4)}";
            else if (telefone.Length == 11)
                return $"({telefone[..2]}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
            

            return null;
        }

    }
}

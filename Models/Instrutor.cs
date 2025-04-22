using System.ComponentModel.DataAnnotations;

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

        private string _cpf;

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone
        {
            get => _telefone;
            set => _telefone = FormatTelefone(value);
        }
        
        private string _telefone;


        private string FormatCpf(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            cpf = cpf.PadLeft(11, '0');

            if (cpf.Length == 11)
            {
                return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
            }

            return cpf;
        }
       

        private string FormatTelefone(string telefone)
        {
            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            if (telefone.Length == 10)
                return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 4)}-{telefone.Substring(6, 4)}"; 
            else if (telefone.Length == 11)
                return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
            

            return telefone;
        }



    }
}

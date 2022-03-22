using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.Models.Atributos;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        [CPF]
        [Required]
        [StringLength(14)]
        public string CPF
        {
            get => cpf;
            set
            {
                Regex regex = new Regex(@"(\d{3})(\d{3})(\d{3})(\d{2})");
                cpf = regex.Replace(value, "$1.$2.$3-$4");
            }
        }
        private string cpf;

        [Required]
        public string Nome { get; set; }
    }
}
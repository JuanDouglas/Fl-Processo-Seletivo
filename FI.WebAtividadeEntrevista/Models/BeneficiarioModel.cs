using System.ComponentModel.DataAnnotations;
using WebAtividadeEntrevista.Models.Atributos;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public int Id { get; set; }
        
        [CPF]
        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        [Required]
        public string Nome { get; set; }
    }
}
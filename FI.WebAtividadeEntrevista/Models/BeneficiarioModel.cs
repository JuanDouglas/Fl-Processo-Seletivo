using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models.Atributos
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
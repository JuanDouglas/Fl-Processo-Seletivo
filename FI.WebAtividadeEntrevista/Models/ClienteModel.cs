﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAtividadeEntrevista.Models.Atributos;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class ClienteModel
    {
        public ClienteModel()
        {
            Beneficiarios = new List<BeneficiarioModel>();
        }

        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [CPF]
        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Required]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Beneficiarios desse cliente
        /// </summary>
        public IEnumerable<BeneficiarioModel> Beneficiarios { get; set; }
    }
}
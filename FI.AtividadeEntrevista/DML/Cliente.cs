using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de cliente que representa o registo na tabela Cliente do Banco de Dados
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// CPF do Beneficiario
        /// </summary>
        public string CPF
        {
            get => cpf; set
            {
                cpf = new string(value
                   .Where(wh => char.IsDigit(wh))
                   .ToArray());
            }
        }

        private string cpf;

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Lista de Beneficiarios de um cliente (Tendo como tipo T <see cref="Beneficiario"/>).
        /// </summary>
        public IEnumerable<Beneficiario> Beneficiarios { get; set; }

        public Cliente()
        {
            Beneficiarios = new List<Beneficiario>();
        }
    }
}

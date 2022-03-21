using System.Linq;
using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe que representa um beneficiaro de um <see cref="Cliente"/>
    /// </summary>
    public class Beneficiario
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF do Beneficiario
        /// </summary>
        public string CPF
        {
            get => cpf;
            set
            {
                cpf = new string(value
                  .Where(wh => char.IsDigit(wh))
                  .ToArray());
            }
        }

        private string cpf;

        /// <summary>
        /// Nome do beneficiario
        /// </summary>
        public string Nome { get; set; }
    }
}

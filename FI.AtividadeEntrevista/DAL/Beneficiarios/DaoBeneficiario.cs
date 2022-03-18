using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Camada de acesso a dados da tabela de beneficiarios
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Insere um novo <see cref="Beneficiario"/> associado a um <see cref="Cliente"/> pelo seu <seealso cref="Cliente.Id"/>.
        /// </summary>
        /// <param name="benef">Objeto do <see cref="Beneficiario"/> que será adicionado.</param>
        /// <param name="idCliente">Id do <see cref="Cliente"/> que será associado</param>
        /// <returns>Id do Beneficiario</returns>
        public long Incluir(Beneficiario benef, long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", benef.CPF),
                new SqlParameter("Nome", benef.Nome),
                new SqlParameter("IdCliente", idCliente)
            };

            DataSet ds = Consultar("FI_SP_IncBenef", parametros);
            long ret = 0;

            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);

            return ret;
        }

        /// <summary>
        /// Verifica se já existe um registro de <see cref="Beneficiario"/> com o <seealso cref="Beneficiario.CPF"/> para o <see cref="Cliente"/> com o <paramref name="idCliente"/> especificado.
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        /// <param name="cpf">CPF do beneficiario a ser buscado</param>
        /// <param name="idBeneficiario">Caso existe retorna o id do beneficiario encontrado</param>
        /// <returns></returns>
        public bool VerificaBeneficiario(long idCliente, string cpf, out long idBeneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", cpf),
                new SqlParameter("IdCliente", idCliente)
            };

            DataSet ds = Consultar("FI_SP_IncBenef", parametros);

            bool existe = ds.Tables[0].Rows.Count > 0;

            if (existe)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out idBeneficiario);
            else
                idBeneficiario = 0;

            return existe;
        }

        /// <summary>
        /// Altera um <see cref="Beneficiario"/> existente.
        /// </summary>
        /// <param name="benef"></param>
        /// <param name="idCliente"></param>
        public void Alterar(Beneficiario benef, long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter> {
                new SqlParameter("Id", benef.Id),
                new SqlParameter("Nome", benef.Nome),
                new SqlParameter("CPF", benef.CPF),
                new SqlParameter("IdlLiente", idCliente)
            };

            Executar("FI_SP_AltBenef", parametros);
        }
    }
}

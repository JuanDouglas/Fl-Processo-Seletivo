using FI.AtividadeEntrevista.DML;
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

            DataSet ds = Consultar("FI_SP_IncBeneficiario", parametros);
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

            DataSet ds = Consultar("FI_SP_VerificaBeneficiario", parametros);

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
                new SqlParameter("IdCliente", idCliente)
            };

            Executar("FI_SP_AltBeneficiario", parametros);
        }

        /// <summary>
        /// Exclui 1 ou todos os <see cref="Beneficiario"/> de um determinado <see cref="Cliente"/>.
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="id"></param>
        public void Excluir(long idCliente, long id = 0)
        {
            List<SqlParameter> parametros = new List<SqlParameter> {
                new SqlParameter("Id", id),
                new SqlParameter("IdCliente", idCliente)
            };

            Executar("FI_SP_DelBeneficiario", parametros);
        }

        public List<Beneficiario> Listar(long idCliente, long id = 0)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id),
                new SqlParameter("IdCliente",idCliente)
            };

            DataSet ds = Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> benefs = Converter(ds);

            return benefs;
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario cli = new Beneficiario
                    {
                        Id = row.Field<long>("Id"),
                        Nome = row.Field<string>("Nome"),
                        CPF = row.Field<string>("CPF")
                    };

                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}

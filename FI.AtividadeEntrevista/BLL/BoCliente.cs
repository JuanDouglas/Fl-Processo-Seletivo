using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    /// <summary>
    /// Camada de lógicas associadas a um cliente.
    /// </summary>
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo <see cref="Cliente"/>
        /// </summary>
        /// <param name="cliente">Objeto de cliente <see cref="Cliente"/></param>
        public long Incluir(Cliente cliente)
        {
            DaoCliente cli = new DaoCliente();
            DaoBeneficiario daoBenef = new DaoBeneficiario();

            ValidarCliente(cliente);

            long idCliente = cli.Incluir(cliente);

            foreach (Beneficiario benef in cliente.Beneficiarios)
            {
                _ = daoBenef.Incluir(benef, idCliente);
            }

            return idCliente;
        }

        /// <summary>
        /// Altera um <see cref="Cliente"/>
        /// </summary>
        /// <param name="cliente">Objeto de <see cref="Cliente"/></param>
        public void Alterar(Cliente cliente)
        {
            DaoCliente cli = new DaoCliente();
            DaoBeneficiario daoBenef = new DaoBeneficiario();

            ValidarCliente(cliente);

            cli.Alterar(cliente);

            foreach (Beneficiario benef in cliente.Beneficiarios)
            {
                long idBenef = benef.Id;
                bool existe = false;

                if (idBenef < 1)
                {
                    existe = daoBenef.VerificaBeneficiario(cliente.Id, benef.CPF, out idBenef);
                }

                if (!existe)
                {
                    _ = daoBenef.Incluir(benef, cliente.Id);
                    continue;
                }

                /*
                 * O uso do ID do cliente nesse estado 
                 * impede que um beneficiario inadequado 
                 * seja alterado.
                 */
                daoBenef.Alterar(benef, cliente.Id);
            }
        }

        /// <summary>
        /// Consulta o <see cref="Cliente"/> pelo <seealso cref="Cliente.Id"/>
        /// </summary>
        /// <param name="id">id do <see cref="Cliente"/></param>
        /// <returns></returns>
        public Cliente Consultar(long id)
        {
            DaoCliente cli = new DaoCliente();
            DaoBeneficiario daoBenef = new DaoBeneficiario();

            Cliente cliente = cli.Consultar(id);
            cliente.Beneficiarios = daoBenef.Listar(id);
            return cliente;
        }

        /// <summary>
        /// Excluir o <see cref="Cliente"/> pelo <seealso cref="Cliente.Id"/>
        /// </summary>
        /// <param name="id">id do <see cref="Cliente"/></param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DaoBeneficiario daoBenef = new DaoBeneficiario();
            DaoCliente cli = new DaoCliente();

            daoBenef.Excluir(id);
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<Cliente> Listar()
        {
            DaoCliente cli = new DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="idExistente">Indica o id de cliente que deve ser desconsiderado.</param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long idExistente = 0)
        {
            DaoCliente cli = new DaoCliente();
            return cli.VerificarExistencia(CPF, idExistente);
        }

        private void ValidarCliente(Cliente cliente)
        {
            DaoCliente cli = new DaoCliente();

            if (cli.VerificarExistencia(cliente.CPF))
            {
                throw new ArgumentException("Não deve haver mais de um registro com o mesmo CPF!");
            }

            foreach (Beneficiario benef in cliente.Beneficiarios)
            {
                int existentes = (from buscado in cliente.Beneficiarios
                                  where benef.CPF == buscado.CPF
                                  select 0).Count();

                if (existentes > 1)
                {
                    throw new ArgumentException("Não deve haver dois beneficiarios com o mesmo CPF");
                }
            }
        }
    }
}

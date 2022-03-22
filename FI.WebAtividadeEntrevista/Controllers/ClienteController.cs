using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Models.Atributos;
using System.Text.RegularExpressions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View(new ClienteModel());
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            ValidarCliente(model);

            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                    Beneficiarios = model.Beneficiarios.Select(sl => new Beneficiario()
                    {
                        Nome = sl.Nome,
                        CPF = sl.CPF
                    }).ToList()
                });

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            ValidarCliente(model);

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                    Beneficiarios = model.Beneficiarios.Select(sl => new Beneficiario()
                    {
                        Nome = sl.Nome,
                        CPF = sl.CPF,
                    }).ToList()
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = cliente.Beneficiarios
                    .Select(sl => new BeneficiarioModel()
                    {
                        CPF = sl.CPF,
                        Nome = sl.Nome,
                        Id = sl.Id
                    })
                    .ToArray()
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult CpfValido(string cpf)
        {
            bool valido = CPFAttribute.CPFValido(cpf);
            string error = valido ? string.Empty : "O CPF informado não é válido!";
            return Json(new
            {
                valido,
                error
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void ValidarCliente(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            Regex regex = new Regex("^[^\\d]$");
            model.CPF = model.CPF ?? string.Empty;
            bool existe = bo.VerificarExistencia(model.CPF, model.Id);
            model.Beneficiarios = model.Beneficiarios ?? new BeneficiarioModel[0];

            if (existe)
            {
                ModelState.AddModelError(nameof(ClienteModel.CPF), "Já existe registro com o CPF informado!");
            }

            for (int i = 0; i < model.Beneficiarios.Count() && ModelState.Count < 1; i++)
            {
                BeneficiarioModel item = model.Beneficiarios[i];
                if (model.Beneficiarios.FirstOrDefault(fs =>
                     regex.Replace(item.CPF ?? string.Empty, string.Empty) == regex.Replace(fs.CPF ?? string.Empty, string.Empty))
                     != null)
                {
                    ModelState.AddModelError(nameof(ClienteModel.Beneficiarios), "Não deve haver dois beneficiários com o mesmo CPF!");
                }
            }

            model.CPF = regex.Replace(model.CPF, string.Empty);
        }
    }
}
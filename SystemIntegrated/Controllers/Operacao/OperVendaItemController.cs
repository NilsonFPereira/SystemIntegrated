using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models.Operacao;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    public class OperVendaItemController : Controller
    {
        private VendaItemRepositorio vendaItemRepositorio;

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult InsertItemSession(VendaItemModel vendaItemModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idItens = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    List<VendaItemModel> lista = (List<VendaItemModel>)Session["itensSession"];


                    if (Session["itensSession"] == null)
                    {
                        lista = new List<VendaItemModel>();
                        lista.Add(vendaItemModel);
                        Session["itensSession"] = lista;

                    }
                    else
                    {
                        lista.Add(vendaItemModel);
                        Session["itensSession"] = lista;
                    }

                    idItens = vendaItemModel.Id.ToString();

                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                    throw new Exception(ex.Source);

                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdItens = idItens });

        }

        [HttpPost]
        [Authorize]
        public JsonResult InserirItem(VendaItemModel vendaItemModel, int idVenda)
        {

            var resultado = "OK";
            var mensagens = string.Empty;

            if (vendaItemModel is null)
            {

                resultado = "AVISO";
                mensagens = "Nenhum produto foi informado.";
            }
            else
            {

                    vendaItemRepositorio = new VendaItemRepositorio();

                    var lista = new List<VendaItemModel>();

                    lista = (List<VendaItemModel>)Session["itensSession"];


                    foreach (var itens in lista)
                    {
                        vendaItemModel = new VendaItemModel()
                        {
                            IdVendaProduto = idVenda,
                            IdProduto = itens.IdProduto,
                            QuantidadeProduto = itens.QuantidadeProduto,
                            ValorUnitarioProduto = itens.ValorUnitarioProduto,
                            ValorDescontoProduto = itens.ValorDescontoProduto,
                            ValorAcrescimoProduto = itens.ValorAcrescimoProduto,
                            ValorTotalProduto = itens.ValorTotalProduto
                        };

                        vendaItemRepositorio.SalvarItens(vendaItemModel);
                        vendaItemRepositorio.AtualizarQuantidadeProduto(vendaItemModel);
                    }

                    Session.Remove("itensSession");

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens });
        }

        [HttpPost]
        [Authorize]
        public JsonResult RecuperarVendaItem(int id)
        {
            vendaItemRepositorio = new VendaItemRepositorio();
            var lista = vendaItemRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }
    }
}
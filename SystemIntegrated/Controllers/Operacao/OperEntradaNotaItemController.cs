using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models.Operacao;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    public class OperEntradaNotaItemController : Controller
    {

        private EntradaNotaItemRepositorio entradaNotaItemRepositorio;

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult InsertItemSession(EntradaNotaItemModel entradaNotaItemModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idItens = string.Empty;

            if ( ! ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    List<EntradaNotaItemModel> lista = (List<EntradaNotaItemModel>)Session["itens"];


                    if (Session["itens"] == null)
                    {
                        lista = new List<EntradaNotaItemModel>();
                        lista.Add(entradaNotaItemModel);
                        Session["itens"] = lista;

                    }
                    else
                    {
                        lista.Add(entradaNotaItemModel);
                        Session["itens"] = lista;
                    }

                    idItens = entradaNotaItemModel.Id.ToString();

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
        public JsonResult InserirItem(EntradaNotaItemModel entradaNotaItemModel, int idEntradaNota)
        {
            var resultado = "OK";
            var mensagens = string.Empty;

            if(entradaNotaItemModel is null) {

                resultado = "AVISO";
                mensagens = "Nenhum produto foi informado.";
            }
            else
            {

                try
                {
                    entradaNotaItemRepositorio = new EntradaNotaItemRepositorio();

                    var lista = (List<EntradaNotaItemModel>)Session["itens"];

                    foreach (var itens in lista)
                    {
                        entradaNotaItemModel = new EntradaNotaItemModel()
                        {
                            IdEntradaNota = itens.IdEntradaNota,
                            IdProduto = itens.IdProduto,
                            QuantidadeProduto = itens.QuantidadeProduto,
                            ValorTotalProduto = itens.ValorTotalProduto,
                            ValorUnitarioProduto = itens.ValorUnitarioProduto
                        };

                        entradaNotaItemRepositorio.SalvarItens(entradaNotaItemModel, idEntradaNota);
                        entradaNotaItemRepositorio.AtualizarQuantidadeProduto(entradaNotaItemModel);
                    }

                    Session.Remove("itens");


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Source);
                }

            }
                
            return Json(new { Resultado = resultado, Mensagens = mensagens });

        }

        [HttpPost]
        [Authorize]
        public JsonResult RecuperarEntradaNotaItem(int id)
        {
            entradaNotaItemRepositorio = new EntradaNotaItemRepositorio();
            var lista = entradaNotaItemRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }
    }
}

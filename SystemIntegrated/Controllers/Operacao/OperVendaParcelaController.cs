using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models.Operacao;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    

    public class OperVendaParcelaController : Controller
    {
        private VendaParcelaRepositorio vendaParcelaRepositorio;
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RecuperarVendaProdutoParcela(int id)
        {
            vendaParcelaRepositorio = new VendaParcelaRepositorio();

            var lista = vendaParcelaRepositorio.RecuperarVendaParcelas(id);

            return Json(lista);

            
        }

        [HttpPost]
        [Authorize]
        public JsonResult InsertSessionParcelas(VendaParcelaModel vendaParcelaModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idParcelas = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(X => X.Errors).Select(X => X.ErrorMessage).ToList();

            }
            else
            {

                try
                {
                    List<VendaParcelaModel> lista = (List<VendaParcelaModel>)Session["parcelas"];

                    if(Session["parcelas"] == null)
                    {
                        lista = new List<VendaParcelaModel>();
                        lista.Add(vendaParcelaModel);
                        Session["parcelas"] = lista;
                    }
                    else
                    {
                        lista.Add(vendaParcelaModel);
                        Session["parcelas"] = lista;
                    }

                    idParcelas = vendaParcelaModel.Id.ToString();


                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                    throw new Exception(ex.Source);

                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdParcelas = idParcelas });
        }

        [HttpPost]
        [Authorize]
        public JsonResult InserirParcela(VendaParcelaModel vendaParcelaModel, int idVenda)
        {

            var resultado = "OK";
            var mensagens = string.Empty;

            if (vendaParcelaModel is null)
            {

                resultado = "AVISO";
                mensagens = "Nenhuma parcela foi informada.";
            }
            else
            {

                vendaParcelaRepositorio = new VendaParcelaRepositorio();

                var lista = new List<VendaParcelaModel>();

                lista = (List<VendaParcelaModel>)Session["parcelas"];


                foreach (var itens in lista)
                {
                    vendaParcelaModel = new VendaParcelaModel()
                    {
                        IdVendaProduto = idVenda,
                        NumeroParcela = itens.NumeroParcela,
                        DataVencimento = itens.DataVencimento,
                        ValorParcela = itens.ValorParcela,
                        ValorAcrescimoParcela = itens.ValorAcrescimoParcela,
                        ValorDescontoParcela = itens.ValorDescontoParcela,
                        ValorTotalParcela = itens.ValorTotalParcela
                    };

                    vendaParcelaRepositorio.SalvarParcelas(vendaParcelaModel);
                }

                Session.Remove("parcelas");

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens });
        }

    }
}
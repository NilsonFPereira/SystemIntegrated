using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    public class OperRecebimentoController : Controller
    {

        private RecebimentoRepositorio recebimentoRepositorio;
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult BuscarVendaCliente(string dadosBusca)
        {
            recebimentoRepositorio = new RecebimentoRepositorio();
            var lista = recebimentoRepositorio.RecuperarVendaClientePelaBusca(dadosBusca);

            return Json(lista);

        }
    }
}
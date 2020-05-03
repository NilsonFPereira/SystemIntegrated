using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}
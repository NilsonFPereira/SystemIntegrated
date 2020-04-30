using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Consulta
{
    public class ConsVendasPorClienteController : Controller
    {
        private ConsultaVendaPorClienteRepositorio consultaVendaPorClienteRepositorio;


        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult VendasPorCliente(string cnpjcpf )
        {
            consultaVendaPorClienteRepositorio = new ConsultaVendaPorClienteRepositorio();
            return Json(consultaVendaPorClienteRepositorio.RecuperarLista(cnpjcpf));



        }
    }
}
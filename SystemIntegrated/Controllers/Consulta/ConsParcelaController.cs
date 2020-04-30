using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Consulta
{
    public class ConsParcelaController : Controller
    {
        private ConsultaParcelaRepositorio consultaParcelaRepositorio;
        // GET: ConsParcela
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ConsultarParcelas(int numeroNota, string cnpjCpf)
        {

            consultaParcelaRepositorio = new ConsultaParcelaRepositorio();
            return Json(consultaParcelaRepositorio.RecuperarLista(numeroNota, cnpjCpf));


        }
    }
}
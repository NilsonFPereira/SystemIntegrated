using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class CadEstadoController : Controller
    {
        private EstadoRepositorio estadoRepositorio;
        private PaisRepositorio paisRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;

        
        public ActionResult Index()
        {
            estadoRepositorio = new EstadoRepositorio();
            paisRepositorio = new PaisRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;


            var lista = estadoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = estadoRepositorio.RecuperarQuantidade();


            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            ViewBag.Paises = paisRepositorio.RecuperarLista();
            
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EstadoPagina(int pagina, int tamPag, string filtro)
        {

            estadoRepositorio = new EstadoRepositorio();
            var lista = estadoRepositorio.RecuperarLista(pagina, tamPag, filtro);
            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarEstado(int id)
        {
            estadoRepositorio = new EstadoRepositorio();

            return Json(estadoRepositorio.RecuperarPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirEstado(int id)
        {
            estadoRepositorio = new EstadoRepositorio();
            return Json(estadoRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarEstado(EstadoModel estadoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    estadoRepositorio = new EstadoRepositorio();

                    var id = estadoRepositorio.Salvar(estadoModel);

                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}
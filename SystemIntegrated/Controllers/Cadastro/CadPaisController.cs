using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadPaisController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        private PaisRepositorio paisRepositorio;

        [Authorize]
        public ActionResult Index()
        {
            paisRepositorio = new PaisRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var lista = paisRepositorio.RecuperarLista(_paginaAtual, _quantMaxLinhasPorPagina);

            var quant = paisRepositorio.RecuperarQuantidade();
            ViewBag.Lista = quant;
            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina)  > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;
            
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult PaisPagina(int pagina, int tamPag, string filtro)
        {

            paisRepositorio = new PaisRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { tamPag, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;

            var lista = paisRepositorio.RecuperarLista(pagina, tamPag, filtro);

            var quant = paisRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarPais(int id)
        {
            paisRepositorio = new PaisRepositorio();
            return Json(paisRepositorio.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarPais(PaisModel paisModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (! ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    paisRepositorio = new PaisRepositorio();

                    var id = paisRepositorio.Salvar(paisModel);

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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirPais(int id)
        {

            paisRepositorio = new PaisRepositorio();

            return Json(paisRepositorio.ExcluirPeloId(id));

        }
    }
}
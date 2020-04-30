using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadNaturezaController : Controller
    {
        private NaturezaRepositorio naturezaRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            naturezaRepositorio = new NaturezaRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = naturezaRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            var lista = naturezaRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult NaturezaPagina(int pagina, int tamPag, string filtro)
        {
            naturezaRepositorio = new NaturezaRepositorio();

            var lista = naturezaRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarNatureza(int id)
        {
            naturezaRepositorio = new NaturezaRepositorio();

            var lista = naturezaRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirNatureza(int id)
        {
            naturezaRepositorio = new NaturezaRepositorio();

            return Json(naturezaRepositorio.ExcluirPeloId(id));


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarNatureza(NaturezaModel naturezaModel)
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
                    naturezaRepositorio = new NaturezaRepositorio();

                    var id = naturezaRepositorio.Salvar(naturezaModel);

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
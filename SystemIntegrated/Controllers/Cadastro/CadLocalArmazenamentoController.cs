using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadLocalArmazenamentoController : Controller
    {
        LocalArmazenamentoRepositorio localArmazenamentoRepositorio;

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = localArmazenamentoRepositorio.RecuperarQuantidade();

            var difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuant;

            var lista = localArmazenamentoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult LocalArmazenamentoPagina(int pagina, int tamPag, string filtro)
        {

            localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();

            var lista = localArmazenamentoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarLocalArmazenamento(int id)
        {
            localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();
            return Json(localArmazenamentoRepositorio.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirLocalArmazenamento(int id)
        {
            localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();
            return Json(localArmazenamentoRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarLocalArmazenamento(LocalArmazenamentoModel localArmazenamentoModel)
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
                    localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();

                    var id = localArmazenamentoRepositorio.Salvar(localArmazenamentoModel);

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
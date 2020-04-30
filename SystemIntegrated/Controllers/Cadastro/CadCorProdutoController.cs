using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadCorProdutoController : Controller
    {
        private CorProdutoRepositorio corProdutoRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            corProdutoRepositorio = new CorProdutoRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = corProdutoRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;


            var lista = corProdutoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult CorProdutoPagina(int pagina, int tamPag, string filtro)
        {
            corProdutoRepositorio = new CorProdutoRepositorio();
            var lista = corProdutoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCorProduto(int id)
        {
            corProdutoRepositorio = new CorProdutoRepositorio();

            var lista = corProdutoRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirCorProduto(int id)
        {
            corProdutoRepositorio = new CorProdutoRepositorio();
            return Json(corProdutoRepositorio.ExcluirPeloId(id));
           
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarCorProduto(CorProdutoModel corProdutoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if(!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }else
            {

                try
                {
                    corProdutoRepositorio = new CorProdutoRepositorio();

                    var id = corProdutoRepositorio.Salvar(corProdutoModel);

                    if(id > 0)
                    {
                        idSalvo = id.ToString();

                    } else
                    {
                        resultado = "ERRO";

                    }

                } catch(Exception ex)
                {

                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}
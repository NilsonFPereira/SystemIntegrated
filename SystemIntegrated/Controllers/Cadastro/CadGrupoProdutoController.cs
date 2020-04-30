using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadGrupoProdutoController : Controller
    {
        private GrupoProdutoRepositorio grupoProdutoRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            grupoProdutoRepositorio = new GrupoProdutoRepositorio();
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.paginaAtual = _paginaAtual;

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);

            var quant = grupoProdutoRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            var lista = grupoProdutoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult GrupoProdutoPagina(int pagina, int tamPag, string filtro)
        {
            grupoProdutoRepositorio = new GrupoProdutoRepositorio();
            var lista = grupoProdutoRepositorio.RecuperarLista(pagina, tamPag, filtro);
            return Json(lista);


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarGrupoProduto(int id)
        {
            grupoProdutoRepositorio = new GrupoProdutoRepositorio();
            var lista = grupoProdutoRepositorio.RecuperarPeloId(id);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirGrupoProduto(int id)
        {
            grupoProdutoRepositorio = new GrupoProdutoRepositorio();
            return Json(grupoProdutoRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarGrupoProduto(GrupoProdutoModel grupoProdutoModel)
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
                    grupoProdutoRepositorio = new GrupoProdutoRepositorio();

                    var id = grupoProdutoRepositorio.Salvar(grupoProdutoModel);

                    if(id > 0)
                    {

                        idSalvo = id.ToString();

                    }
                    else
                    {

                        resultado = "ERRO";

                    }
                }catch(Exception ex)
                {

                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                }
            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}
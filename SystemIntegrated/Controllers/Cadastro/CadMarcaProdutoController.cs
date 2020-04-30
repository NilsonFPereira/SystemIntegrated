using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadMarcaProdutoController : Controller
    {
        private MarcaProdutoRepositorio marcaProdutoRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;


        [Authorize]
        public ActionResult Index()
        {
            marcaProdutoRepositorio = new MarcaProdutoRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = marcaProdutoRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            
            var lista = marcaProdutoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult MarcaProdutoPagina(int pagina, int tamPag, string filtro)
        {
            marcaProdutoRepositorio = new MarcaProdutoRepositorio();

            var lista = marcaProdutoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarMarcaProduto(int id)
        {
            marcaProdutoRepositorio = new MarcaProdutoRepositorio();

            var lista = marcaProdutoRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirMarcaProduto(int id)
        {
            marcaProdutoRepositorio = new MarcaProdutoRepositorio();

            return Json(marcaProdutoRepositorio.ExcluirPeloId(id));


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarMarcaProduto(MarcaProdutoModel marcaProdutoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if( ! ModelState.IsValid )
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {
                try
                {
                    marcaProdutoRepositorio = new MarcaProdutoRepositorio();

                    var id = marcaProdutoRepositorio.Salvar(marcaProdutoModel);

                    if(id > 0)
                    {

                        idSalvo = id.ToString();

                    }
                    else
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
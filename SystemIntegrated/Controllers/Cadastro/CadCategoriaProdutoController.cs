using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    [Authorize(Roles = "ADMINISTRADOR,OPERADOR,GERENTE")]
    public class CadCategoriaProdutoController : Controller
    {
        CategoriaProdutoRepositorio categoriaProdutoRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        
        public ActionResult Index()
        {
            categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = categoriaProdutoRepositorio.RecuperarQuantidade();

            var difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuant;

            var lista = categoriaProdutoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCategoriaProduto(int id)
        {
            categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();
            return Json(categoriaProdutoRepositorio.RecuperarPeloId(id));
        }

        [HttpPost]     
        [ValidateAntiForgeryToken]
        public JsonResult CategoriaProdutoPagina(int pagina, int tamPag, string filtro)
        {

            categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();

            var lista = categoriaProdutoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarCategoriaProduto(CategoriaProdutoModel categoriaProdutoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if( !ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();

                    var id = categoriaProdutoRepositorio.Salvar(categoriaProdutoModel);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirCategoriaProduto(int id)
        {
            categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();
            return Json(categoriaProdutoRepositorio.ExcluirPeloId(id));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadFretePorContaController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        private FretePorContaRepositorio fretePorContaRepositorio;
        
        [Authorize]
        public ActionResult Index()
        {
            fretePorContaRepositorio = new FretePorContaRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;
            
            var lista = fretePorContaRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = fretePorContaRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % _quantMaxLinhasPorPagina ) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult FretePorCOntaPagina(int pagina, int tamPag, string filtro)
        {

            fretePorContaRepositorio = new FretePorContaRepositorio();

            var lista = fretePorContaRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarFretePorConta(int id)
        {
            fretePorContaRepositorio = new FretePorContaRepositorio();

            return Json(fretePorContaRepositorio.RecuperarPeloId(id));

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirFretePorConta( int id)
        {
            fretePorContaRepositorio = new FretePorContaRepositorio();

            return Json(fretePorContaRepositorio.ExcluirPeloId(id));


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarFretePorConta(FretePorContaModel fretePorContaModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if( ! ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {

                try
                {
                    fretePorContaRepositorio = new FretePorContaRepositorio();
                    var id = fretePorContaRepositorio.Salvar(fretePorContaModel);

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
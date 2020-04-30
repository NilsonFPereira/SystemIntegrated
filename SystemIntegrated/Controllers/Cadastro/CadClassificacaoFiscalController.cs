using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadClassificacaoFiscalController : Controller
    {
        private ClassificacaoFiscalRepositorio classificacaoFiscalRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20, _quantMaxLinhasPorPagina });
            ViewBag.QuantLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = classificacaoFiscalRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantLinhasPorPagina) + ViewBag.difQuantPaginas;

            var lista = classificacaoFiscalRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarClassificacaoFiscal(int id)
        {

               classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();
            return Json(classificacaoFiscalRepositorio.RecuperarPeloId(id));


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ClassificacaoFiscalPagina( int pagina, int tamPag, string filtro)
        {

            classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();

            var lista = classificacaoFiscalRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarClassificacaoFiscal(ClassificacaoFiscalModel classificacaoFiscalModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if(!ModelState.IsValid)
            {

                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            
            } else
            {

                try
                {
                    classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();

                    var id = classificacaoFiscalRepositorio.Salvar(classificacaoFiscalModel);

                    if( id > 0)
                    {

                        idSalvo = id.ToString();

                    }else
                    {

                        resultado = "ERRO";
                    }


                } catch( Exception ex)
                {

                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                }


            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo}); 
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirClassificacaoFiscal(int id)
        {
            classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();
            return Json(classificacaoFiscalRepositorio.ExcluirPeloId(id));
        }
    }
}
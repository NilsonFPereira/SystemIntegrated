using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    public class CadTipoPessoaController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        private TipoPessoaRepositorio tipoPessoaRepositorio;

        [Authorize]
        public ActionResult Index()
        {
            tipoPessoaRepositorio = new TipoPessoaRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;


            var lista = tipoPessoaRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = tipoPessoaRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarTipoPessoa(int id)
        {
            tipoPessoaRepositorio = new TipoPessoaRepositorio();
            return Json(tipoPessoaRepositorio.RecuperarPeloId(id));


        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult TipoPessoaPagina(int pagina, int tamPag, string filtro)
        {
            tipoPessoaRepositorio = new TipoPessoaRepositorio();


            var lista = tipoPessoaRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirTipoPessoa(int id)
        {
            tipoPessoaRepositorio = new TipoPessoaRepositorio();
            return Json(tipoPessoaRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarTipoPessoa(TipoPessoaModel tipoPessoaModel)
        {
            var resultado = "OK";
            var mensagens = new List<String>();
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
                    tipoPessoaRepositorio = new TipoPessoaRepositorio();
                    var id = tipoPessoaRepositorio.Salvar(tipoPessoaModel);

                    if( id > 0)
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
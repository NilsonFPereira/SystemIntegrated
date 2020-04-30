using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models.Cadastro;
using SystemIntegrated.Repositorio.Cadastro;

namespace SystemIntegrated.Controllers.Cadastro
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class CadFormaPagamentoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;
        private FormaPagamentoRepositorio formaPagamentoRepositorio;
      
        public ActionResult Index()
        {
            formaPagamentoRepositorio = new FormaPagamentoRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = formaPagamentoRepositorio.RecuperarQuantidade();

            var difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuant;

            var lista = formaPagamentoRepositorio.RecuperarLista();


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarFormaPagamento(int id)
        {
            formaPagamentoRepositorio = new FormaPagamentoRepositorio();
            var lista = formaPagamentoRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FormaPagamentoPagina(int pagina, int tamPag, string filtro)
        {
            formaPagamentoRepositorio = new FormaPagamentoRepositorio();

            var lista = formaPagamentoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarFormaPagamento(FormaPagamentoModel formaPagamentoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if( ! ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }else
            {
                try
                {
                    formaPagamentoRepositorio = new FormaPagamentoRepositorio();
                    var id = formaPagamentoRepositorio.Salvar(formaPagamentoModel);

                    if( id > 0 )
                    {
                        idSalvo = id.ToString();

                    }else
                    {

                        resultado = "ERRO";
                    }

                } catch(Exception ex)
                {

                    resultado = "ERRO";

                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirFormaPagamento(int id)
        {

            formaPagamentoRepositorio = new FormaPagamentoRepositorio();

            return Json( formaPagamentoRepositorio.ExcluirPeloId(id) );

        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models.Operacao;
using SystemIntegrated.Repositorio;
using SystemIntegrated.Repositorio.Cadastro;
//using SystemIntegrated.Repositorio.Cadastro;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    public class OperVendaController : Controller
    {
        private VendaRepositorio vendaRepositorio;
        private FretePorContaRepositorio fretePorContaRepositorio;
        private FormaPagamentoRepositorio formaPagamentoRepositorio;

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        [Authorize]
        public ActionResult Index()
        {
            vendaRepositorio = new VendaRepositorio();
            fretePorContaRepositorio = new FretePorContaRepositorio();
            formaPagamentoRepositorio = new FormaPagamentoRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            ViewBag.FormasPagamento = formaPagamentoRepositorio.RecuperarLista();

            ViewBag.FretePorConta = fretePorContaRepositorio.RecuperarLista();

            var quant = vendaRepositorio.RecuperarQuantidade();

            var lista = vendaRepositorio.RecuperarLista();

            var difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuant;

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarVendaProduto(VendaModel vendaModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if(!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {

                try
                {
                    vendaRepositorio = new VendaRepositorio();

                    var id = vendaRepositorio.Salvar(vendaModel);

                    if(id > 0)
                    {

                        idSalvo = id.ToString();

                    }else
                    {

                        resultado = "ERRO";

                    }


                }catch(Exception ex)
                {

                    resultado = "ERRO";

                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}

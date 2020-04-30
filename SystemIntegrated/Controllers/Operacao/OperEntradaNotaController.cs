using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Models.Operacao;
using SystemIntegrated.Repositorio;
using SystemIntegrated.Repositorio.Operacao;

namespace SystemIntegrated.Controllers.Operacao
{
    public class OperEntradaNotaController : Controller
    {

        private EntradaNotaRepositorio entradaNotaRepositorio;
        private NaturezaRepositorio naturezaRepositorio;
        private FretePorContaRepositorio fretePorContaRepositorio;
        private FornecedorRepositorio fornecedorRepositorio;
        
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;


        public ActionResult Index()
        {
            entradaNotaRepositorio = new EntradaNotaRepositorio();
            naturezaRepositorio = new NaturezaRepositorio();
            fretePorContaRepositorio = new FretePorContaRepositorio();
            

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            ViewBag.Natureza = naturezaRepositorio.RecuperarLista();
            ViewBag.FretePorConta = fretePorContaRepositorio.RecuperarLista();
            

            var quant = entradaNotaRepositorio.RecuperarQuantidade();
            ViewBag.Lista = quant;

            ViewBag.difQuant = (ViewBag.Lista % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (ViewBag.Lista / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuant;


            var lista = entradaNotaRepositorio.RecuperarLista();

            return View(lista);

           

        }

        [HttpPost]
        [Authorize]
        public JsonResult RemoteData(string query)
        {
            List<FornecedorModel> listData = null;

            if (!string.IsNullOrEmpty(query))
            {

                fornecedorRepositorio = new FornecedorRepositorio();
                listData = fornecedorRepositorio.ListaSuggest(query);

            }

            return Json(listData);
        }

        [HttpPost]
        [Authorize]
        public JsonResult SalvarEntradaNota(EntradaNotaModel entradaNotaModel)
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

                try { 

                    entradaNotaRepositorio = new EntradaNotaRepositorio();

                    var id = entradaNotaRepositorio.Salvar(entradaNotaModel);

                if (id > 0)
                {

                    idSalvo = id.ToString();

                }else
                {
                    resultado = "ERRO";

                }
                } catch(Exception ex)
                {

                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                }
            }
            return Json( new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        public JsonResult RecuperarEntradaNota(int id)
        {

            entradaNotaRepositorio = new EntradaNotaRepositorio();
            var lista = entradaNotaRepositorio.RecuperarPeloId(id);

            return Json(lista);
        }


    }

}
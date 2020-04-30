using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class CadFornecedorController : Controller
    {
        private FornecedorRepositorio fornecedorRepositorio;
        private TipoPessoaRepositorio tipoPessoaRepositorio;
        private EstadoRepositorio estadoRepositorio;
        private CidadeRepositorio cidadeRepositorio;
        private SexoRepositorio sexoRepositorio;

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        
        public ActionResult Index()
        {
            fornecedorRepositorio = new FornecedorRepositorio();
            tipoPessoaRepositorio = new TipoPessoaRepositorio();
            estadoRepositorio = new EstadoRepositorio();
            cidadeRepositorio = new CidadeRepositorio();
            sexoRepositorio = new SexoRepositorio();

            ViewBag.ListaTamPag = new SelectList (new int[] {  _quantMaxLinhasPorPagina, 10, 15, 20}, _quantMaxLinhasPorPagina);
            ViewBag.QuantLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = fornecedorRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantLinhasPorPagina) + ViewBag.difQuantPaginas;

            ViewBag.TipoPessoa = tipoPessoaRepositorio.RecuperarLista();
            ViewBag.Estado = estadoRepositorio.RecuperarLista();            
            ViewBag.Sexo = sexoRepositorio.RecuperarLista();

            var lista = fornecedorRepositorio.RecuperarLista(@ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }
        
        [HttpPost]
        public JsonResult FornecedorPagina(int pagina, int tamPag, string filtro)
        {
            fornecedorRepositorio = new FornecedorRepositorio();

            var lista = fornecedorRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        public JsonResult RecuperarFornecedor(int id)
        {
            fornecedorRepositorio = new FornecedorRepositorio();

            return Json(fornecedorRepositorio.RecuperarPeloId(id));

        }

        [HttpPost]
        public JsonResult ExcluirFornecedor(int id)
        {
            fornecedorRepositorio = new FornecedorRepositorio();

            return Json(fornecedorRepositorio.ExcluirPeloId(id));

        }


        [HttpPost]
        public JsonResult SalvarFornecedor(FornecedorModel fornecedorModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if ( ! ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }else
            {

                try {

                    fornecedorRepositorio = new FornecedorRepositorio();

                    var id = fornecedorRepositorio.Salvar(fornecedorModel);

                if(id > 0)
                {

                    idSalvo = id.ToString();

                } else
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


        [HttpPost]
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
    }
}
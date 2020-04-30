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
    public class CadProdutoController : Controller
    {
        private ProdutoRepositorio produtoRepositorio;
        private UnidadeMedidaRepositorio unidadeMedidaRepositorio;
        private GrupoProdutoRepositorio grupoProdutoRepositorio;
        private CategoriaProdutoRepositorio categoriaProdutoRepositorio;
        private MarcaProdutoRepositorio marcaProdutoRepositorio;
        private FornecedorRepositorio fornecedorRepositorio;
        private CorProdutoRepositorio corProdutoRepositorio;
        private ClassificacaoFiscalRepositorio classificacaoFiscalRepositorio;
        private LocalArmazenamentoRepositorio localArmazenamentoRepositorio;

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        public ActionResult Index()
        {
            produtoRepositorio = new ProdutoRepositorio();
            unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();
            grupoProdutoRepositorio = new GrupoProdutoRepositorio();
            marcaProdutoRepositorio = new MarcaProdutoRepositorio();
            fornecedorRepositorio = new FornecedorRepositorio();
            categoriaProdutoRepositorio = new CategoriaProdutoRepositorio();
            corProdutoRepositorio = new CorProdutoRepositorio();
            classificacaoFiscalRepositorio = new ClassificacaoFiscalRepositorio();
            localArmazenamentoRepositorio = new LocalArmazenamentoRepositorio();


            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = produtoRepositorio.RecuperarQuantidade();

            var difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuant;

            ViewBag.UnidadeMedida = unidadeMedidaRepositorio.RecuperarLista(1, 9999);
            ViewBag.Grupos = grupoProdutoRepositorio.RecuperarLista(1,9999);
            ViewBag.Categorias = categoriaProdutoRepositorio.RecuperarLista(1,9999);            
            ViewBag.Marcas = marcaProdutoRepositorio.RecuperarLista(1,9999);
            ViewBag.Fornecedores = fornecedorRepositorio.RecuperarLista(1,9999);
            ViewBag.CorProduto = corProdutoRepositorio.RecuperarLista(1,9999);
            ViewBag.Classificacao = classificacaoFiscalRepositorio.RecuperarLista(1,9999);
            ViewBag.LocalArmazenamento = localArmazenamentoRepositorio.RecuperarLista();

            var lista = produtoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarProduto(int id)
        {
            produtoRepositorio = new ProdutoRepositorio();

            var lista = produtoRepositorio.RecuperarPeloId(id);

            return Json(lista);
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProdutoPagina(int pagina, int tamPag, string filtro)
        {
            produtoRepositorio = new ProdutoRepositorio();

            var lista = produtoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarProduto(ProdutoModel produtoModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if ( ! ModelState.IsValid)
            {

                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {
                try
                {
                    produtoRepositorio = new ProdutoRepositorio();

                    var id = produtoRepositorio.Salvar(produtoModel);

                    if(id > 0)
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
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirProduto(int id)
        {
            produtoRepositorio = new ProdutoRepositorio();

            return Json(produtoRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]  
        public JsonResult RemoteData(string query)
        {
            List<ProdutoViewModel> listData = null;

            if (!string.IsNullOrEmpty(query))
            {

                produtoRepositorio = new ProdutoRepositorio();
                listData = produtoRepositorio.ListaSuggest(query);

            }

            return Json(listData);
        }
    }
}
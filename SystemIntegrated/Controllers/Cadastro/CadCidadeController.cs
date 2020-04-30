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
    public class CadCidadeController : Controller
    {
        
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        private CidadeRepositorio cidadeRepositorio;
        private EstadoRepositorio estadoRepositorio;

        public ActionResult Index()
        {
            estadoRepositorio = new EstadoRepositorio();
            cidadeRepositorio = new CidadeRepositorio();

            ViewBag.ListaTamPag = new SelectList( new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;


            var lista = cidadeRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = cidadeRepositorio.RecuperarQuantidade();
            
            ViewBag.Estados = estadoRepositorio.RecuperarLista();


            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            
            ViewBag.QuantPaginas = ( quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CidadePagina(int pagina, int tamPag, string filtro)
        {
            cidadeRepositorio = new CidadeRepositorio();

 
            var lista = cidadeRepositorio.RecuperarLista(pagina, tamPag, filtro);
            
            return  Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCidade(int id)
        {

            cidadeRepositorio = new CidadeRepositorio();
            return Json(cidadeRepositorio.RecuperarPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarCidade(CidadeModel cidadeModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if ( !ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {
                try
                {
                    cidadeRepositorio = new CidadeRepositorio();

                    var id = cidadeRepositorio.Salvar(cidadeModel);

                    if(id > 0)
                    {

                        idSalvo = id.ToString();
                    } else
                    {

                        resultado = "ERRO";
                    }

                } 
                catch(Exception ex)
                {
                    resultado = "ERRO";
                    throw new Exception(ex.Source);
                    

                }

            }

            return Json (new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirCidade(int id)
        {

            cidadeRepositorio = new CidadeRepositorio();

            return Json(cidadeRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCidadesDoEstado(int idEstado)
        {
            cidadeRepositorio = new CidadeRepositorio();

            var lista = cidadeRepositorio.RecuperarLista(idEstado: idEstado);

            return Json(lista);
        }
    }
}
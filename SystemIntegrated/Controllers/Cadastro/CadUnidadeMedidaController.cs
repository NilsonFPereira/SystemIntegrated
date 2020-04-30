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
    public class CadUnidadeMedidaController : Controller
    {
        private UnidadeMedidaRepositorio unidadeMedidaRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;
        
        public ActionResult Index()
        {
            unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = unidadeMedidaRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            var lista = unidadeMedidaRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarUnidadeMedida(int id)
        {
            unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();

            var lista = unidadeMedidaRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UnidadeMedidaPagina(int pagina, int tamPag, string filtro)
        {
            unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();

            var lista = unidadeMedidaRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarUnidadeMedida(UnidadeMedidaModel unidadeMedidaModel)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {
                try
                {
                    unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();

                    var id = unidadeMedidaRepositorio.Salvar(unidadeMedidaModel);

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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirUnidadeMedida(int id)
        {
            unidadeMedidaRepositorio = new UnidadeMedidaRepositorio();

            return Json(unidadeMedidaRepositorio.ExcluirPeloId(id));


        }
    }
}
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
    public class CadSexoController : Controller
    {

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAutal = 1;

        private SexoRepositorio sexoRepositorio;

        public ActionResult Index()
        {

            sexoRepositorio = new SexoRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAutal;

            var lista = sexoRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = sexoRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;


            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarSexo(int id)
        {
            sexoRepositorio = new SexoRepositorio();
            return Json(sexoRepositorio.RecuperarPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SexoPagina(int pagina, int tamPag, string filtro)
        {
            sexoRepositorio = new SexoRepositorio();
            var lista = sexoRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarSexo( SexoModel sexoModel)
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
                    sexoRepositorio = new SexoRepositorio();

                    var id = sexoRepositorio.Salvar(sexoModel);

                    if( id > 0 )
                    {
                        idSalvo = id.ToString();


                    }else
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
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirSexo(int id)
        {
            sexoRepositorio = new SexoRepositorio();

            return Json(sexoRepositorio.ExcluirPeloId(id));

        }
    }
}
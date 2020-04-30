using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers.Cadastro
{
    [Authorize(Roles ="ADMINISTRADOR")]
    public class CadUsuarioController : Controller
    {

        private UsuarioRepositorio usuarioRepositorio;

        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        
        public ActionResult Index()
        {
            usuarioRepositorio = new UsuarioRepositorio();


            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = usuarioRepositorio.RecuperarQuantidade();

            ViewBag.difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuant;
            var lista = usuarioRepositorio.RecuperarLista();
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarUsuario(int id)
        {
            usuarioRepositorio = new UsuarioRepositorio();
            var lista = usuarioRepositorio.RecuperarPeloId(id);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag, string filtro)
        {
            usuarioRepositorio = new UsuarioRepositorio();
            var lista = usuarioRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirUsuario(int id)
        {
            usuarioRepositorio = new UsuarioRepositorio();

            return Json(usuarioRepositorio.ExcluirPeloId(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarUsuario(UsuarioModel usuarioModel)
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
                try { 
                    usuarioRepositorio = new UsuarioRepositorio();

                    var id = usuarioRepositorio.Salvar(usuarioModel);

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

    }
}
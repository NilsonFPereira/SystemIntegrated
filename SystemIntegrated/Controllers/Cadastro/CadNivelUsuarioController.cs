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
    public class CadNivelUsuarioController : Controller
    {
        private NivelUsuarioRepositorio nivelUsuarioRepositorio;
        private UsuarioRepositorio usuarioRepositorio;
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        public ActionResult Index()
        {
            nivelUsuarioRepositorio = new NivelUsuarioRepositorio();
            usuarioRepositorio = new UsuarioRepositorio();

            ViewBag.ListaUsuario = usuarioRepositorio.RecuperarLista();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = nivelUsuarioRepositorio.RecuperarQuantidade();

            ViewBag.difQuant = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuant;

            var lista = nivelUsuarioRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarNivelUsuario(int id)
        {
            nivelUsuarioRepositorio = new NivelUsuarioRepositorio();


            var lista = nivelUsuarioRepositorio.RecuperarPeloId(id);

            lista.CarregarUsuarios();

            return Json(lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NivelUsuarioPagina(int pagina, int tamPag, string filtro)
        {
            nivelUsuarioRepositorio = new NivelUsuarioRepositorio();

            var lista = nivelUsuarioRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarNivelUsuario(NivelUsuarioModel nivelUsuarioModel, List<int> idUsuarios)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {

                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }else
            {
                nivelUsuarioModel.Usuarios = new List<UsuarioModel>();


                foreach(var id in idUsuarios)
                {
                    nivelUsuarioModel.Usuarios.Add(new UsuarioModel() { Id = id });
                }


                try
                {
                    nivelUsuarioRepositorio = new NivelUsuarioRepositorio();

                    var id = nivelUsuarioRepositorio.Salvar(nivelUsuarioModel);

                    if( id > 0)
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
        public JsonResult ExcluirNivelUsuario(int id)
        {
            nivelUsuarioRepositorio = new NivelUsuarioRepositorio();
            return Json(nivelUsuarioRepositorio.ExcluirPeloId(id));

        }
    }
}
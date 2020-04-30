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
    public class CadClienteController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const int _paginaAtual = 1;

        private ClienteRepositorio    clienteRepositorio;
        private TipoPessoaRepositorio tipoPessoaRepositorio;
        private SexoRepositorio       sexoRepositorio;
        private EstadoRepositorio     estadoRepositorio;
        public ActionResult Index()
        {
            clienteRepositorio    = new ClienteRepositorio();
            tipoPessoaRepositorio = new TipoPessoaRepositorio();
            sexoRepositorio       = new SexoRepositorio();
            estadoRepositorio     = new EstadoRepositorio();

            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = _paginaAtual;

            var quant = clienteRepositorio.RecuperarQuantidade();

            ViewBag.difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + ViewBag.difQuantPaginas;

            var lista = clienteRepositorio.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            

            ViewBag.TipoPessoa = tipoPessoaRepositorio.RecuperarLista();
            ViewBag.Sexo       = sexoRepositorio.RecuperarLista();
            ViewBag.Estado     = estadoRepositorio.RecuperarLista();

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ClientePagina(int pagina, int tamPag, string filtro)
        {
            clienteRepositorio = new ClienteRepositorio();
            var lista = clienteRepositorio.RecuperarLista(pagina, tamPag, filtro);

            return Json(lista);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCliente(int id)
        {
            clienteRepositorio = new ClienteRepositorio();

            return Json(clienteRepositorio.RecuperarPeloId(id));

        }


        [HttpPost]        
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirCliente(int id)
        {

            clienteRepositorio = new ClienteRepositorio();

            return Json(clienteRepositorio.ExcluirPeloId(id));


        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public JsonResult SalvarCliente(ClienteModel clienteModel)
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
                    clienteRepositorio = new ClienteRepositorio();

                    var id = clienteRepositorio.Salvar(clienteModel);

                    if(id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
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
        public JsonResult RemoteData(string query)
        {
            List<ClienteModel> listData = null;

            if (!string.IsNullOrEmpty(query))
            {

                clienteRepositorio = new ClienteRepositorio();
                listData = clienteRepositorio.ListaSuggest(query);

            }

            return Json(listData);
        }
    }
}
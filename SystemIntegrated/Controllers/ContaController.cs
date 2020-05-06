using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated.Controllers
{

    [Authorize(Roles = "ADMINISTRADOR,OPERADOR,GERENTE")]
    public class ContaController : Controller
    {

        private UsuarioRepositorio usuarioRepositorio;
        private NivelUsuarioRepositorio nivelUsuarioRepositorio;

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);

            }

            usuarioRepositorio = new UsuarioRepositorio();
            nivelUsuarioRepositorio = new NivelUsuarioRepositorio();

            var usuario = (usuarioRepositorio.ValidarUsuario(login.Usuario, login.Senha));

            if (usuario != null)
            {
                var ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                   1,usuario.Nome,DateTime.Now,DateTime.Now.AddHours(12),login.LembrarMe,usuario.Id + "|" + usuarioRepositorio.RecuperarStringPerfil(usuario.Id)));

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);

                Response.Cookies.Add(cookie);


                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }


            }
            else
            {
                ModelState.AddModelError("", "Login Inválido");

            }
            return View(login);

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public ActionResult AlterarSenhaUsuario(AlterarSenhaUsuarioViewModel model)
        {

            if (HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var usuarioLogado = (HttpContext.User as AplicacaoPrincipal);
                var alterou = false;
                usuarioRepositorio = new UsuarioRepositorio();
                if (usuarioLogado != null)
                {
                    if ( ! usuarioRepositorio.ValidarSenhaAtual(model.SenhaAtual, usuarioLogado.Dados.Id))
                    {

                        ModelState.AddModelError("SenhaAtual", "A senha atual não confere.");

                    }
                    else { 
                    
                        alterou = usuarioRepositorio.AlterarSenha(model.NovaSenha, usuarioLogado.Dados.Id);

                        if (alterou)
                        {
                            ViewBag.Mensagens = new String[] { "ok", "Senha alterada com sucesso." };
                        }
                        else
                        {
                            ViewBag.Mensagens = new String[] { "erro", "Não foi possível alterar a senha." };
                        }
                    
                    }
                }
                return View();

            }
            else
            {

                ModelState.Clear();
                return View();

            }
        }




    }
}
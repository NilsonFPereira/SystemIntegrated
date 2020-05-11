using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [AllowAnonymous]
        public ActionResult EsqueciMinhaSenha(EsqueciMinhaSenhaViewModel model)
        {
            ViewBag.EmailEnviado = true;

            if(HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {

                ViewBag.EmailEnviado = false;
                ModelState.Clear();

            }
            else
            {
                usuarioRepositorio = new UsuarioRepositorio();

                var usuario = usuarioRepositorio.RecuperarPeloLogin(model.Login);

                if(usuario != null)
                {
                    
                    EnviarEmailRefefinicaoSenha(usuario);
                }

            }
            return View(model);

        }

        private void EnviarEmailRefefinicaoSenha(UsuarioModel usuario)
        {
            var callbackUrl = Url.Action("RedefinirSenha", "Conta", new { id = usuario.Id }, protocol: Request.Url.Scheme);
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = ConfigurationManager.AppSettings["EmailServidor"];
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPorta"]);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailUsuario"],
                ConfigurationManager.AppSettings["EmailSenha"]
            );


            MailMessage mensagem = new MailMessage();

            mensagem.From = new MailAddress(ConfigurationManager.AppSettings["EmailOrigem"], "SystemIntegrated - Sistema Integrado");
            mensagem.To.Add(usuario.Email);
            mensagem.Subject = "Redefinição de Senha";
            mensagem.Body = string.Format("Redefina a sua senha <a href='{0}'>aqui</a>", callbackUrl);
            mensagem.IsBodyHtml = true;
            mensagem.Priority = MailPriority.High;

            try { 

                client.Send(mensagem);
            
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {

                mensagem = null;

            }
        }

        [AllowAnonymous]
        public ActionResult RedefinirSenha(int id)
        {
            usuarioRepositorio = new UsuarioRepositorio();

            var usuario = usuarioRepositorio.RecuperarPeloId(id);

            if (usuario == null)
            {
                id = -1;
            }

            var model = new NovaSenhaViewModel() { Usuario = id };

            ViewBag.Mensagem = null;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RedefinirSenha(NovaSenhaViewModel model)
        {
            ViewBag.Mensagem = null;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            usuarioRepositorio = new UsuarioRepositorio();
            var usuario = usuarioRepositorio.RecuperarPeloId(model.Usuario);

            if (usuario != null)
            {
                var ok = usuarioRepositorio.AlterarSenha(model.Senha, usuario.Id);
                ViewBag.Mensagem = ok ? "Senha alterada com sucesso!" : "Não foi possível alterar a senha!";
            }

            return View();
        }

        private void EnviarEmailRedefinicaoSenha(UsuarioModel usuario)
        {
            var callbackUrl = Url.Action("RedefinirSenha", "Conta", new { id = usuario.Id }, protocol: Request.Url.Scheme);
            var client = new SmtpClient()
            {
                Host = ConfigurationManager.AppSettings["EmailServidor"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPorta"]),
                EnableSsl = (ConfigurationManager.AppSettings["EmailSsl"] == "S"),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["EmailUsuario"],
                    ConfigurationManager.AppSettings["EmailSenha"])
            };

            var mensagem = new MailMessage();
            mensagem.From = new MailAddress(ConfigurationManager.AppSettings["EmailOrigem"], "Controle de Estoque - Como Programar Melhor");
            mensagem.To.Add(usuario.Email);
            mensagem.Subject = "Redefinição de senha";
            mensagem.Body = string.Format("Redefina a sua senha <a href='{0}'>aqui</a>", callbackUrl);
            mensagem.IsBodyHtml = true;

            client.Send(mensagem);
        }
    }
}
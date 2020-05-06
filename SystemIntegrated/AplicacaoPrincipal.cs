using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SystemIntegrated.Models;
using SystemIntegrated.Repositorio;

namespace SystemIntegrated
{
    public class AplicacaoPrincipal : GenericPrincipal
    {
        private UsuarioRepositorio usuarioRepositorio;
        public UsuarioModel Dados { get; set; }

        public AplicacaoPrincipal(IIdentity identity, string[] roles, int id): base(identity, roles)
        {
            usuarioRepositorio = new UsuarioRepositorio();
            Dados = usuarioRepositorio.RecuperarPeloId(id);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class NivelUsuarioViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    }
}
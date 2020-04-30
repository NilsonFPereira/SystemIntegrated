using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class CidadeViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string NomeEstado { get; set; }
        public string Sigla { get; set; }
    }
}
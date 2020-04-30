using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class EstadoViewModel
    {
        public int Id { get; set; }

        
        public string Codigo { get; set; }
        
        public string Nome { get; set; }
        
        public string Sigla { get; set; }
        
        public string NomePais { get; set; }
        public bool Ativo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class CorProdutoModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Informe o nome.")]       
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    }
}
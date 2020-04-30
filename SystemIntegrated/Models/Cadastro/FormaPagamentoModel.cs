using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models.Cadastro
{
    public class FormaPagamentoModel
    {
        public int Id { get; set; }
                
        [Required(ErrorMessage="Informe o código.")]
        
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        
        public string Nome { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class EstadoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o código.")]
        
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        
        public string Nome { get; set; }

        [Required(ErrorMessage ="Informe a sigla.")]
        
        public string Sigla { get; set; }

        [Required(ErrorMessage = "Selecione o pais.")]
        public int IdPais { get; set; }

        
        public string NomePais { get; set; }
        public bool Ativo { get; set; }
    }
}
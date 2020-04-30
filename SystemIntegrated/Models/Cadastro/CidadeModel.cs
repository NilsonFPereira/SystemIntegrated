using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class CidadeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Selecione o estado.")]
        public int IdEstado { get; set; }
        public bool Ativo { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class PaisModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [StringLength(80)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha a sigla.")]
        [StringLength(2)]
        public string Sigla { get; set; }
        public bool Ativo { get; set; }

    }
}
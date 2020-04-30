using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage ="Informe o Usuário.")]
        [Display(Name = "Usuário")]
        public string Usuario { get; set; }
        
        [Required(ErrorMessage = "Informe a Senha.")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public bool LembrarMe { get; set; }
    }
}
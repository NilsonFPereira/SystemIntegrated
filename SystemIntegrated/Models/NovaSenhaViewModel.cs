﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class NovaSenhaViewModel
    {
        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Informe a confirmação da senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha:")]
        public string ConfirmacaoSenha { get; set; }

        public int Usuario { get; set; }
    }
}
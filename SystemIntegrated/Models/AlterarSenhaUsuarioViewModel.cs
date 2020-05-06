using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class AlterarSenhaUsuarioViewModel
    {
        [Required(ErrorMessage = "Preencha a senha atual.")]
        [Display(Name = "Senha Atual")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Preenha a nova senha.")]
        [MinLength(3, ErrorMessage = "A nova senha deve ter no mínimo 3 caracteres")]
        [Display(Name = "Nova Senha.")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Preencha a confirmação da nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "A senha e a confirmação devem ser iguais.")]
        [Display(Name = "Confirmação da nova senha.")]
        public string ConfirmacaoNovaSenha { get; set; }

    }
}
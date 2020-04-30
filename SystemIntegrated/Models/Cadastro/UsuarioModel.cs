using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o usuário.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Informe o cpf.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Informe o email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe a data de cadastro.")]
        public string DataCadastro { get; set; }
        public string DataVencimento { get; set; }
        public bool Ativo { get; set; }

    }
}
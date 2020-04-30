using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class FornecedorModel
    {
        public int Id { get; set; }

        public int IdTipoPessoa { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        public string Nome { get; set; }

        public string RazaoSocial { get; set; }
        public string IdSexo { get; set; }

        public string DataNascimento { get; set; }

        [Required(ErrorMessage = "Informe o cnpj/cpf.")]        
        public string CnpjCpf { get; set; }

        [Required(ErrorMessage = "Informe o rg/inscricao estadual.")]        
        public string RgInscricaoEstadual { get; set; }

        [Required(ErrorMessage = "Selecione o estado.")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "Selecione a cidade.")]
        public int IdCidade { get; set; }

        [Required(ErrorMessage = "Informe o logradouro.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Informe o número.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe o bairro.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage ="Informe o cep.")]
        public string Cep { get; set; }
        public string Telefone { get; set; }      
        public string Celular { get; set; }        
        public string Fax { get; set; }        
        public string Email { get; set; }
        public string Site { get; set; }

        [Required(ErrorMessage = "Informe a data de cadastro.")]
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }

    }
}
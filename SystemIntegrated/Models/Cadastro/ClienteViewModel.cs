using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIntegrated.Models
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        public int IdTipoPessoa { get; set; }
        public string TipoPessoa { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string IdSexo { get; set; }
        public string Sexo { get; set; }
        public string DataNascimento { get; set; }
        public string CnpjCpf { get; set; }
        public string RgInscricaoEstadual { get; set; }
        public int IdEstado { get; set; }
        public string NomeEstado { get; set; }
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }        
        public string Celular { get; set; }        
        public string Email { get; set; }
        public string DataCadastro { get; set; }
        public bool Ativo { get; set; }

    }
}
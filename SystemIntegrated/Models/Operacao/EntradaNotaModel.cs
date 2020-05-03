using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class EntradaNotaModel
    {
        public int Id {get; set;}

        [Required(ErrorMessage="Informe a data de emissão.")]
        public string DataEmissao {get; set;}

        [Required(ErrorMessage = "Informe o número da nota.")]
        public string NumeroNota {get; set;}

        [Required(ErrorMessage = "Informe a natureza.")]
        public int IdNatureza { get; set; }

        [Required(ErrorMessage = "Informe a chave de acesso.")]
        public string ChaveAcesso {get; set;}

        [Required(ErrorMessage = "informe o valor total dos produtos.")]
        public string ValorTotalProdutos { get; set; }

        [Required(ErrorMessage = "Informe o fornecedor.")]
        public int IdFornecedor {get; set;}

        [Required(ErrorMessage = "selecione o responsável pelo frete.")]
        public int IdFretePorConta {get; set;}

        [Required(ErrorMessage = "Informe o valor do frete.")]
        public string ValorFrete { get; set; }

        [Required(ErrorMessage = "Informe o valor do desconto.")]
        public string ValorDesconto { get; set; }

        [Required(ErrorMessage = "Informe o valor do icms.")]
        public string ValorIcms { get; set; }

        [Required(ErrorMessage = "Informe o valor do ipi.")]
        public string ValorIpi { get; set; }

        [Required(ErrorMessage = "Informe o valor total.")]
        public string ValorTotalNota {get; set;}
        public string DataEntrada { get; set; }
        public bool EhCancelada {get; set;}
    
    }
}
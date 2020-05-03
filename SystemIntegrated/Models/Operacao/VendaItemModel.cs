using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaItemModel
    {
        public int Id { get; set; }
        public int IdVendaProduto { get; set; }

        [Required(ErrorMessage = "Informe o produto.")]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "Informe a quantidade.")]
        public decimal QuantidadeProduto { get; set; }

        [Required(ErrorMessage = "Informe o valor unitário.")]
        public string ValorUnitarioProduto { get; set; }

        [Required(ErrorMessage = "Informe o valor total dos produtos.")]
        public string ValorTotalProduto { get; set; }

        [Required(ErrorMessage = "Informe o valor de desconto.")]
        public string ValorDescontoProduto { get; set; }

        [Required(ErrorMessage = "Informe o valor de acréscimo.")]
        public string ValorAcrescimoProduto { get; set; }

    }
}
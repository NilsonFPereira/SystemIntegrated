using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class EntradaNotaItemModel
    {
        public int Id { get; set; }
        public int IdEntradaNota { get; set; }

        [Required(ErrorMessage="Informe o produto.")]
        public int IdProduto { get; set; }

        [Required(ErrorMessage="Informe a quantidade.")]
        public decimal QuantidadeProduto { get; set; }

        [Required(ErrorMessage="Informe o valor unitário.")]
        public string ValorUnitarioProduto { get; set; }

        [Required(ErrorMessage = "Informe o valor total dos produtos.")]
        public string ValorTotalProduto { get; set; }

    }
}
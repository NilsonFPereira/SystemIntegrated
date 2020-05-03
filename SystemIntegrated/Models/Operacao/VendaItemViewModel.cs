using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaItemViewModel
    {
        public int IdVendaItem {get; set;}
        public int IdProduto {get; set;}
        public decimal QuantidadeProduto {get; set;}
        public string NomeProduto {get; set;}
        public string UnidadeMedida {get; set;}
        public string ValorUnitarioProduto {get; set;}
        public string ValorDescontoProduto { get; set; }
        public string ValorAcrescimoProduto { get; set; }
        public string ValorTotalProduto { get; set; }
    }
}
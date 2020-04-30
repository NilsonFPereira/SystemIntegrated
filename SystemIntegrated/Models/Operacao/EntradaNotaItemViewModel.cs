using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class EntradaNotaItemViewModel
    {
        public int Id { get; set; }
        public int IdNotaItem {get; set;}       
        public int IdProduto {get; set;}       
        public decimal QuantidadeProduto {get; set;}       
        public string NomeProduto {get; set;}       
        public string UnidadeMedida {get; set;}       
        public string ValorUnitarioProduto {get; set;}       
        public string ValorTotalProduto { get; set;}       

    }
}
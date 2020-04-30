using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaViewModel
    {
        public int Id { get; set; }
        public string DataVenda { get; set; }
        public int IdCliente { get; set; }
        public string NumeroVenda { get; set; }
        public string ValorTotalNota { get; set; }
        public string ValorDesconto { get; set; }
        public int IdFormaPagamento { get; set; }
        public string ValorPago { get; set; }
        public string ValorFrete { get; set; }
        public int IdFretePorConta { get; set; }
        public string ValorProduto { get; set; }
        public string DataCadastro { get; set; }
    }
}
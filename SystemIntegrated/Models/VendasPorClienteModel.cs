using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class VendasPorClienteModel
    {
        public int IdVendaProduto { get; set; }
        public string DataVenda { get; set; }
        public string NumeroVenda { get; set; }

        public decimal ValorTotalNota { get; set; }

        public int NumeroParcela { get; set; }

        public string DataVencimento { get; set; }

        public decimal ValorParcela { get; set; }

        public string DataPagamento { get; set; }

        public string StatusPagamento { get; set; }

        public string CnpjCpf { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class ParcelaModel
    {
        public string Nome { get; set; }
        public int NumeroParcela { get; set; }
        public string DataVencimento { get; set; }

        public decimal ValorParcela { get; set; }

        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }

        public string DataPagamento { get; set; }

        public string StatusPagamento { get; set; }

        public string CnpjCpf { get; set; }

        public int NumeroNota { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Celular { get; set; }

        public string NumeroVenda { get; set; }



    }
}
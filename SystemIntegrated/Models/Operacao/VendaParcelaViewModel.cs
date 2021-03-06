﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaParcelaViewModel
    {
        public string Parcela { get; set; }
        public int IdParcela { get; set; }
        public int IdVendaProduto { get; set; }
        public int NumeroParcela { get; set; }
        public string ValorParcela { get; set; }
        public string ValorAcrescimoParcela { get; set; }
        public string ValorDescontoParcela { get; set; }
        public string ValorTotalParcela { get; set; }
        public string DataVencimento { get; set; }
    }
}
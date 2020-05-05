using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaClienteViewModel
    {
        public int Id { get; set; }
        public string CnpjCpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string NumeroVenda { get; set; }
        public string DataVenda { get; set; }
        public string ValorTotalNota { get; set; }
        public string ValorPago { get; set; }

    }
}
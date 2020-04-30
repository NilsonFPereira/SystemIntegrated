using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class EntradaNotaViewModel
    {
        public int Id { get; set; }
        public int IdNatureza { get; set; }
        public int IdFornecedor { get; set; }
        public int IdFretePorConta { get; set; }
        public string NomeFornecedor { get; set; }
        public string DataEmissao { get; set; }
        public string NumeroNota { get; set; }
        public string ChaveAcesso { get; set; }
        public string ValorTotalProdutos { get; set; }
        public string ValorTotalNota { get; set; }
        public string ValorDesconto { get; set; }
        public string ValorFrete { get; set; }
        public string ValorIcms { get; set; }
        public string ValorIpi { get; set; }

        public string DataEntrada { get; set; }
        public bool EhCancelada { get; set; }
    }
}
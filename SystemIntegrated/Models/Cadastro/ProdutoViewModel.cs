using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int QuantEstoque{ get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public bool Ativo { get; set; }

        public string Sigla { get; set; }
    }
}
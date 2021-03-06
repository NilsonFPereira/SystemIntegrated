﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models.Operacao
{
    public class VendaModel
    {
        public int Id { get; set; }

       [Required(ErrorMessage = "Informe a data da venda.")]
        public string DataVenda { get; set; }

        [Required(ErrorMessage = "Informe o cliente.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Informe o número da venda.")]
        public string NumeroVenda { get; set; }

        [Required(ErrorMessage = "Informe o valor total da nota.")]
        public string ValorTotalNota { get; set; }

        [Required(ErrorMessage = "Informe o valor do desconto.")]
        public string ValorDesconto { get; set; }

        [Required(ErrorMessage = "Informe a forma de pagamento.")]
        public int IdFormaPagamento { get; set; }

        [Required(ErrorMessage = "Informe o valor pago.")]
        public string ValorPago { get; set; }

        [Required(ErrorMessage = "Informe o valor do frete.")]
        public string ValorFrete { get; set; }

        [Required(ErrorMessage = "Selecione o responsável pelo frete.")]
        public int IdFretePorConta { get; set; }

        [Required(ErrorMessage = "Informe o valor dos produtos.")]
        public string ValorProduto { get; set; }

        [Required(ErrorMessage = "Informe a data de cadastro.")]
        public string DataCadastro { get; set; }

        [Required(ErrorMessage = "Informe o valor do acrescimo.")]

        public string ValorAcrescimo { get; set; }

    }
}
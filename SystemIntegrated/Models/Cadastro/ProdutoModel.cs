using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o preço de custo.")]
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "Preencha o preço de venda.")]
        public decimal PrecoVenda { get; set; }

        [Required(ErrorMessage = "Preencha a quantidade em estoque.")]
        public int QuantEstoque { get; set; }

        [Required(ErrorMessage = "Selecione a unidade de medida.")]
        public int IdUnidadeMedida { get; set; }

        [Required(ErrorMessage = "Selecione o grupo de produtos.")]
        public int IdGrupoProduto { get; set; }

        [Required(ErrorMessage = "Selecione a categoria do produto.")]
        public int IdCategoriaProduto { get; set; }

        [Required(ErrorMessage = "Selecione a marca do produto.")]
        public int IdMarcaProduto { get; set; }

        [Required(ErrorMessage = "Selecione o fornecedor.")]
        public int IdFornecedor { get; set; }

        [Required(ErrorMessage = "Selecione a classificação fiscal.")]
        public int IdClassificacaoFiscal { get; set; }

        [Required(ErrorMessage = "Selecione a cor do produto.")]
        public int IdCor { get; set; }

        [Required(ErrorMessage = "Selecione o local de armazenamento.")]
        public int IdLocalArmazenamento { get; set; }

        public bool Ativo { get; set; }

    }
}
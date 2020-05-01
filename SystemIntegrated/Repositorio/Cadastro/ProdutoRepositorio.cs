using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemIntegrated.Models;

namespace SystemIntegrated.Repositorio
{
    public class ProdutoRepositorio
    {
        private SqlConnection con;
        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }
        public List<ProdutoViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<ProdutoViewModel>();

            Connection();
            var paginacao = "";
            var pos = (pagina - 1) * tamPag;
            if(pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }

            var filtroWhere = "";

            if(!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }
            using(SqlCommand command = new SqlCommand(string.Format("    SELECT * " +
                                                                    "      FROM Produto" +
                                                                                filtroWhere +
                                                                    " ORDER BY Codigo DESC "+
                                                                                paginacao ), con))
            {
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new ProdutoViewModel()
                    {
                        Id = (int) reader["Id"],
                        Codigo = (string) reader["Codigo"],
                        Nome = (string) reader["Nome"],
                        QuantEstoque = (int) reader["QuantEstoque"],
                        PrecoCusto = (decimal) reader["PrecoCusto"],
                        PrecoVenda = (decimal) reader["PrecoVenda"],
                        Ativo = (bool) reader["Ativo"]
                    });

                }
                return ret;
            }
        }
        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();
            using(SqlCommand command = new SqlCommand(" SELECT COUNT(*)" +
                                                      "   FROM Produto ", con))
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }

            return ret;
        }

        public ProdutoModel RecuperarPeloId(int id)
        {
            ProdutoModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT Id,                                       " +
                                                      "        Nome,                                     " +
                                                      "        IdFornecedor,                             " +
                                                      "        IdGrupoProduto,                           " +
                                                      "        IdCategoriaProduto,                       " +
                                                      "        IdUnidadeMedida,                          " +
                                                      "        IdClassificacaoFiscal,                    " +
                                                      "        IdCor,                                    " +
                                                      "        Ativo,                                    " +
                                                      "        Codigo,                                   " +
                                                      "        PrecoCusto = Replace(PrecoCusto,'.',','), " +
                                                      "        PrecoVenda = Replace(PrecoVenda,'.',','), " +
                                                      "        QuantEstoque,                             " +
                                                      "        IdLocalArmazenamento,                     " +
                                                      "        IdMarcaProduto                            " +
                                                      "   FROM Produto                                   " +
                                                      "  WHERE Id=@Id                                    ", con))
            {

                con.Open();
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if(reader.Read())
                {
                    ret = new ProdutoModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"],
                        IdFornecedor = (int)reader["IdFornecedor"],
                        IdGrupoProduto = (int)reader["IdGrupoProduto"],
                        IdCategoriaProduto = (int)reader["IdCategoriaProduto"],
                        IdUnidadeMedida = (int)reader["IdUnidadeMedida"],
                        IdClassificacaoFiscal = (int)reader["IdClassificacaoFiscal"],
                        IdCor = (int)reader["IdCor"],
                        IdMarcaProduto = (int) reader["IdMarcaProduto"],
                        Ativo = (bool)reader["Ativo"],
                        PrecoCusto = (string)reader["PrecoCusto"],
                        PrecoVenda = (string)reader["PrecoVenda"],
                        QuantEstoque = (int)reader["QuantEstoque"],
                        IdLocalArmazenamento = (int)reader["IdLocalArmazenamento"]
                    };
                }
            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using(SqlCommand command = new SqlCommand("DELETE Produto WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar(ProdutoModel produtoModel)
        {
            var ret = 0;

            var modal = RecuperarPeloId(produtoModel.Id);

            if (modal == null)
            {
                Connection();

                using (SqlCommand command = new SqlCommand(" INSERT INTO Produto (  Codigo,                  " +
                                                           "                        Nome,                    " +
                                                           "                        PrecoCusto,              " +
                                                           "                        PrecoVenda,              " +
                                                           "                        QuantEstoque,            " +
                                                           "                        IdUnidadeMedida,         " +
                                                           "                        IdGrupoProduto,          " +
                                                           "                        IdCor,                   " +
                                                           "                        IdCategoriaProduto,      " +
                                                           "                        IdClassificacaoFiscal,   " +
                                                           "                        IdLocalArmazenamento,    " +
                                                           "                        IdMarcaProduto,          " +
                                                           "                        IdFornecedor,            " +
                                                           "                        Ativo                    " +
                                                           "                      )                          " +
                                                           "               VALUES ( @Codigo,                 " +
                                                           "                        @Nome,                   " +
                                                           "                        @PrecoCusto,             " +
                                                           "                        @PrecoVenda,             " +
                                                           "                        @QuantEstoque,           " +
                                                           "                        @IdUnidadeMedida,        " +
                                                           "                        @IdGrupoProduto,         " +
                                                           "                        @IdCor,                  " +
                                                           "                        @IdCategoriaProduto,     " +
                                                           "                        @IdClassificacaoFiscal,  " +
                                                           "                        @IdLocalArmazenamento,   " +
                                                           "                        @IdMarcaProduto,         " +
                                                           "                        @IdFornecedor,           " +
                                                           "                        @Ativo                   " +
                                                           "                      );                         " +
                                                           " select convert(int, scope_identity())            ", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = produtoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = produtoModel.Nome;
                    command.Parameters.AddWithValue("@PrecoCusto", SqlDbType.Decimal).Value = produtoModel.PrecoCusto;
                    command.Parameters.AddWithValue("@PrecoVenda", SqlDbType.Decimal).Value = produtoModel.PrecoVenda;
                    command.Parameters.AddWithValue("@QuantEstoque", SqlDbType.Int).Value = produtoModel.QuantEstoque;
                    command.Parameters.AddWithValue("@IdUnidadeMedida", SqlDbType.Int).Value = produtoModel.IdUnidadeMedida;
                    command.Parameters.AddWithValue("@IdGrupoProduto", SqlDbType.Int).Value = produtoModel.IdGrupoProduto;
                    command.Parameters.AddWithValue("@IdCor", SqlDbType.Int).Value = produtoModel.IdCor;
                    command.Parameters.AddWithValue("@IdCategoriaProduto", SqlDbType.Int).Value = produtoModel.IdCategoriaProduto;
                    command.Parameters.AddWithValue("@IdClassificacaoFiscal", SqlDbType.Int).Value = produtoModel.IdClassificacaoFiscal;
                    command.Parameters.AddWithValue("@IdLocalArmazenamento", SqlDbType.Int).Value = produtoModel.IdLocalArmazenamento;
                    command.Parameters.AddWithValue("@IdMarcaProduto", SqlDbType.Int).Value = produtoModel.IdMarcaProduto;
                    command.Parameters.AddWithValue("@IdFornecedor", SqlDbType.Int).Value = produtoModel.IdFornecedor;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = produtoModel.Ativo;

                    ret = (int) command.ExecuteScalar();

                }
            }
            else
            {
                Connection();

                using(SqlCommand command = new SqlCommand("UPDATE Produto                                       " +
                                                          "   SET Codigo=@Codigo,                               " +
                                                          "       Nome=@Nome,                                   " +
                                                          "       PrecoCusto=@PrecoCusto,                       " +
                                                          "       PrecoVenda=@PrecoVenda,                       " +
                                                          "       QuantEstoque=@QuantEstoque,                   " +
                                                          "       IdUnidadeMedida=@IdUnidadeMedida,             " +
                                                          "       IdGrupoProduto=@IdGrupoProduto,               " +
                                                          "       IdCor=@IdCor,                                 " +
                                                          "       IdCategoriaProduto=@IdCategoriaProduto,       " +
                                                          "       IdClassificacaoFiscal=@IdClassificacaoFiscal, " +
                                                          "       IdLocalArmazenamento=@IdLocalArmazenamento,   " +
                                                          "       IdMarcaProduto=@IdMarcaProduto,               " +
                                                          "       IdFornecedor=@IdFornecedor,                   " +
                                                          "       Ativo=@Ativo                                  " +
                                                          " WHERE Id=@Id                                        ", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = produtoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = produtoModel.Nome;
                    command.Parameters.AddWithValue("@PrecoCusto", SqlDbType.Decimal).Value = produtoModel.PrecoCusto;
                    command.Parameters.AddWithValue("@PrecoVenda", SqlDbType.Decimal).Value = produtoModel.PrecoVenda;
                    command.Parameters.AddWithValue("@QuantEstoque", SqlDbType.Int).Value = produtoModel.QuantEstoque;
                    command.Parameters.AddWithValue("@IdUnidadeMedida", SqlDbType.Int).Value = produtoModel.IdUnidadeMedida;
                    command.Parameters.AddWithValue("@IdGrupoProduto", SqlDbType.Int).Value = produtoModel.IdGrupoProduto;
                    command.Parameters.AddWithValue("@IdCor", SqlDbType.Int).Value = produtoModel.IdCor;
                    command.Parameters.AddWithValue("@IdCategoriaProduto", SqlDbType.Int).Value = produtoModel.IdCategoriaProduto;
                    command.Parameters.AddWithValue("@IdClassificacaoFiscal", SqlDbType.Int).Value = produtoModel.IdClassificacaoFiscal;
                    command.Parameters.AddWithValue("@IdLocalArmazenamento", SqlDbType.Int).Value = produtoModel.IdLocalArmazenamento;
                    command.Parameters.AddWithValue("@IdMarcaProduto", SqlDbType.Int).Value = produtoModel.IdMarcaProduto;
                    command.Parameters.AddWithValue("@IdFornecedor", SqlDbType.Int).Value = produtoModel.IdFornecedor;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = produtoModel.Ativo;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = produtoModel.Id;

                    if(command.ExecuteNonQuery() > 0)
                    {

                        ret = produtoModel.Id;

                    }
                }
            }                                  
            return ret;
        }

        public List<ProdutoViewModel> ListaSuggest(string query)
        {
            var ret = new List<ProdutoViewModel>();

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(query))
            {

                filtroWhere = string.Format(" WHERE  LOWER(PR.Nome) LIKE '%{0}%'", query.ToLower());
            }


            Connection();

            using (SqlCommand command = new SqlCommand("     SELECT PR.Id,                                         " +
                                                       "            PR.Nome,                                       " +
                                                       "            PR.QuantEstoque,                               " +
                                                       "            Sigla = UN.Sigla                               " +
                                                       "       FROM Produto PR                                     " +
                                                       " INNER JOIN UnidadeMedida UN ON UN.Id = PR.IdUnidadeMedida " +
                                                                    filtroWhere                                      +
                                                      "    ORDER BY PR.Nome                                        ", con))
            {

                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new ProdutoViewModel()
                    {

                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        QuantEstoque = (int) reader["QuantEstoque"],
                        Sigla = (string) reader["Sigla"]

                    });
                }
            }

            return ret;

        }
    }
}
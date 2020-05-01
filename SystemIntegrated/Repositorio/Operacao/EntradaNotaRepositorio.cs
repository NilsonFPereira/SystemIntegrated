using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemIntegrated.Models.Operacao;

namespace SystemIntegrated.Repositorio.Operacao
{
    public class EntradaNotaRepositorio
    {
        private SqlConnection con;
        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<EntradaNotaViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<EntradaNotaViewModel>();            

            Connection();

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(NumeroNota) LIKE '%{0}%'", filtro.ToLower());

            }

            var paginacao = "";

            var pos = (pagina - 1) * tamPag;

            if (pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }

            using (SqlCommand command = new SqlCommand(String.Format("   SELECT Id,                                                                         " +
                                                                     "          DataEmissao = CONVERT(VARCHAR(10), DataEmissao, 103 ),                      " +
                                                                     "          NumeroNota,                                                                 " +
                                                                     "          ChaveAcesso,                                                                " +
                                                                     "          ValorTotalProdutos = CONVERT(VARCHAR, REPLACE(ValorTotalProdutos,'.',',')), " +
                                                                     "          ValorTotalNota = CONVERT(VARCHAR, REPLACE(ValorTotalNota,'.',',')),         " +
                                                                     "          ValorDesconto = CONVERT(VARCHAR, REPLACE(ValorDesconto,'.',',')),           " +
                                                                     "          ValorFrete = CONVERT(VARCHAR, REPLACE(ValorFrete,'.',',')),                 " +
                                                                     "          ValorIcms = CONVERT(VARCHAR, REPLACE(ValorIcms,'.',',')),                   " +
                                                                     "          ValorIpi = CONVERT(VARCHAR, REPLACE(ValorIpi,'.',','))                      " +
                                                                     "     FROM EntradaNota                                                                 " +
                                                                                filtroWhere                                                                   +
                                                                     " ORDER BY Id DESC                                                                     " +
                                                                                paginacao 
                                                                    ), con ) ) 
            {
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new EntradaNotaViewModel()
                    {
                        Id                 = (int)    reader["Id"],
                        DataEmissao        = (string) reader["DataEmissao"],
                        NumeroNota         = (string) reader["NumeroNota"],
                        ChaveAcesso        = (string) reader["ChaveAcesso"],
                        ValorTotalProdutos = (string) reader["ValorTotalProdutos"],
                        ValorTotalNota     = (string) reader["ValorTotalNota"],
                        ValorDesconto      = (string) reader["ValorDesconto"],
                        ValorFrete         = (string) reader["ValorFrete"],
                        ValorIcms          = (string) reader["ValorIcms"],
                        ValorIpi           = (string) reader["ValorIpi"]                        
                    });
                }
            }
            return ret;
        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using(SqlCommand command = new SqlCommand("SELECT COUNT(*)    " +
                                                      "  FROM EntradaNota ", con))
            {
                con.Open();

                ret = (int)command.ExecuteScalar();
            }
            return ret;
        }

        public EntradaNotaViewModel RecuperarPeloId(int id)
        {
            EntradaNotaViewModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand("     SELECT EN.Id,                                                       " +
                                                      "            DataEmissao = CONVERT(VARCHAR(10), EN.DataEmissao, 103),     " +
                                                      "            EN.IdNatureza,                                               " +
                                                      "            EN.NumeroNota,                                               " +
                                                      "            EN.ChaveAcesso,                                              " +
                                                      "            EN.IdFornecedor,                                             " +
                                                      "            EN.IdFretePorConta,                                          " +
                                                      "            ValorTotalProdutos = CONVERT(VARCHAR, REPLACE(EN.ValorTotalProdutos,'.',',')), " +
                                                      "            ValorTotalNota = CONVERT(VARCHAR, REPLACE(EN.ValorTotalNota,'.',',')),                                        " +
                                                      "            ValorDesconto = CONVERT(VARCHAR, REPLACE(EN.ValorDesconto,'.',',')),                                         " +
                                                      "            ValorFrete = CONVERT(VARCHAR, REPLACE(EN.ValorFrete,'.',',')),                                            " +
                                                      "            ValorIcms = CONVERT(VARCHAR, REPLACE(EN.ValorIcms,'.',',')),                                             " +
                                                      "            ValorIpi = CONVERT(VARCHAR, REPLACE(EN.ValorIpi,'.',',')),                                             " +
                                                      "            DataEntrada = CONVERT(VARCHAR(10), EN.DataEntrada, 103),     " +
                                                      "            EN.EhCancelada,                                              " +
                                                      "            NomeFornecedor = FO.Nome                                     " +
                                                      "       FROM EntradaNota EN                                            " +
                                                      " INNER JOIN Fornecedor FO ON FO.Id = EN.IdFornecedor                  " +
                                                      "      WHERE EN.Id=@Id                                                    ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new EntradaNotaViewModel() { 
                        
                    Id = (int) reader["Id"],
                    DataEmissao = (string) reader["DataEmissao"],
                    IdNatureza = (int)reader["IdNatureza"],
                    NumeroNota = (string) reader["NumeroNota"],
                    ChaveAcesso = (string) reader["ChaveAcesso"],
                    IdFornecedor = (int) reader["IdFornecedor"],
                    IdFretePorConta = (int) reader["IdFretePorConta"],
                    ValorTotalProdutos = (string) reader["ValorTotalProdutos"],
                    ValorTotalNota = (string)reader["ValorTotalNota"],
                    ValorDesconto = (string)reader["ValorDesconto"],
                    ValorFrete = (string)reader["ValorFrete"],
                    ValorIcms = (string)reader["ValorIcms"],
                    ValorIpi = (string)reader["ValorIpi"],
                    DataEntrada = (string)reader["DataEntrada"],
                    EhCancelada = (bool)reader["EhCancelada"],
                    NomeFornecedor = (string) reader["NomeFornecedor"]

                    };
                }
            }
            return ret;
        }

        public int Salvar(EntradaNotaModel entradaNotaModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(entradaNotaModel.Id);

            if(model == null)
            {
                Connection();

                using(SqlCommand command = new SqlCommand("INSERT INTO EntradaNota ( DataEmissao,        " +
                                                          "                          IdNatureza,         " +
                                                          "                          NumeroNota,         " +
                                                          "                          ChaveAcesso,        " +
                                                          "                          IdFornecedor,       " +
                                                          "                          IdFretePorConta,    " +
                                                          "                          ValorTotalProdutos, " +
                                                          "                          ValorTotalNota,     " +
                                                          "                          ValorDesconto,      " +
                                                          "                          ValorFrete,         " +
                                                          "                          ValorIcms,          " +
                                                          "                          ValorIpi,           " +
                                                          "                          DataEntrada,        " +
                                                          "                          EhCancelada         " +
                                                          "                       )                      " +
                                                          "                VALUES ( @DataEmissao,        " +
                                                          "                         @IdNatureza,         " +
                                                          "                         @NumeroNota,         " +
                                                          "                         @ChaveAcesso,        " +
                                                          "                         @IdFornecedor,       " +
                                                          "                         @IdFretePorConta,    " +
                                                          "                         @ValorTotalProdutos, " +
                                                          "                         @ValorTotalNota,     " +
                                                          "                         @ValorDesconto,      " +
                                                          "                         @ValorFrete,         " +
                                                          "                         @ValorIcms,          " +
                                                          "                         @ValorIpi,           " +
                                                          "                         @DataEntrada,        " +
                                                          "                         @EhCancelada         " +
                                                          "                       );                     " +
                                                          " select convert(int, scope_identity())        ", con ))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@DataEmissao",        SqlDbType.VarChar).Value = entradaNotaModel.DataEmissao;
                    command.Parameters.AddWithValue("@IdNatureza",         SqlDbType.Int     ).Value = entradaNotaModel.IdNatureza;
                    command.Parameters.AddWithValue("@NumeroNota",         SqlDbType.VarChar ).Value = entradaNotaModel.NumeroNota;
                    command.Parameters.AddWithValue("@ChaveAcesso",        SqlDbType.VarChar ).Value = entradaNotaModel.ChaveAcesso;
                    command.Parameters.AddWithValue("@IdFornecedor",       SqlDbType.Int     ).Value = entradaNotaModel.IdFornecedor;
                    command.Parameters.AddWithValue("@IdFretePorConta",    SqlDbType.Int     ).Value = entradaNotaModel.IdFretePorConta;
                    command.Parameters.AddWithValue("@ValorTotalProdutos", SqlDbType.Decimal ).Value = entradaNotaModel.ValorTotalProdutos;
                    command.Parameters.AddWithValue("@ValorTotalNota",     SqlDbType.Decimal ).Value = entradaNotaModel.ValorTotalNota;
                    command.Parameters.AddWithValue("@ValorDesconto",      SqlDbType.Decimal ).Value = entradaNotaModel.ValorDesconto;
                    command.Parameters.AddWithValue("@ValorFrete",         SqlDbType.Decimal ).Value = entradaNotaModel.ValorFrete;
                    command.Parameters.AddWithValue("@ValorIcms",          SqlDbType.Decimal ).Value = entradaNotaModel.ValorIcms;
                    command.Parameters.AddWithValue("@ValorIpi",           SqlDbType.Decimal ).Value = entradaNotaModel.ValorIpi;
                    command.Parameters.AddWithValue("@DataEntrada",        SqlDbType.VarChar).Value = entradaNotaModel.DataEntrada;
                    command.Parameters.AddWithValue("@EhCancelada",        SqlDbType.Bit     ).Value = entradaNotaModel.EhCancelada;

                    ret = (int)command.ExecuteScalar();

                }

            }else
            {
                Connection();

                ret = entradaNotaModel.Id;

            }

            return ret;

        }
    }
}
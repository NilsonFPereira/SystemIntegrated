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
    public class EntradaNotaItemRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {

            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public int SalvarItens(EntradaNotaItemModel entradaNotaItemModel , int idNotaItem)
        {

            var ret = 0;

            Connection();


                using(SqlCommand command = new SqlCommand("INSERT INTO EntradaNotaItem ( IdEntradaNota,         " +
                                                          "                              IdProduto,             " +
                                                          "                              Quantidade,            " +
                                                          "                              ValorTotalProduto,     " +
                                                          "                              ValorUnitarioProduto   " +
                                                          "                             )                       " +
                                                          "                      VALUES(                        " +
                                                          "                                                     " +
                                                          "                              @IdEntradaNota,        " +
                                                          "                              @IdProduto,            " +
                                                          "                              @Quantidade,           " +
                                                          "                              @ValorTotalProduto,    " +
                                                          "                              @ValorUnitarioProduto  " +
                                                          "                            )                        ", con))
                {
                    con.Open();


                    command.Parameters.AddWithValue("@IdEntradaNota", SqlDbType.Int).Value = idNotaItem;
                    command.Parameters.AddWithValue("@IdProduto", SqlDbType.Int).Value = entradaNotaItemModel.IdProduto;
                    command.Parameters.AddWithValue("@Quantidade", SqlDbType.VarChar).Value = entradaNotaItemModel.QuantidadeProduto;
                    command.Parameters.AddWithValue("@ValorTotalProduto", SqlDbType.Int).Value = entradaNotaItemModel.ValorTotalProduto;
                    command.Parameters.AddWithValue("@ValorUnitarioProduto", SqlDbType.Int).Value = entradaNotaItemModel.ValorUnitarioProduto;

                    command.ExecuteScalar();

                }
            return ret;
        }

        public int AtualizarQuantidadeProduto(EntradaNotaItemModel entradaNotaItemModel)
        {
            var ret = 0;

            Connection();

           using(SqlCommand command = new SqlCommand(" UPDATE Produto                                     " +
                                                     "    SET QuantEstoque = QuantEstoque + @QuantEstoque " +
                                                     "  WHERE Id = @Id                                    ", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@QuantEstoque", SqlDbType.Int).Value = entradaNotaItemModel.QuantidadeProduto;
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = entradaNotaItemModel.IdProduto;

                command.ExecuteNonQuery();
            }


            return ret;
        }

        public List<EntradaNotaItemViewModel>RecuperarPeloId(int id)
        {
            var ret = new List<EntradaNotaItemViewModel>();

            Connection();

            using(SqlCommand command = new SqlCommand("SELECT IdNotaItem = EI.Id, "+
                                                      " IdProduto = EI.IdProduto, " +
                                                      " NomeProduto = PR.Nome, " +
                                                      " UnidadeMedida = UM.Sigla, " +
                                                      " QuantidadeProduto = EI.Quantidade, "+
                                                      " ValorUnitarioProduto = REPLACE(ValorUnitarioProduto, '.', ','), " +
                                                      " ValorTotalProduto = REPLACE(ValorTotalProduto, '.', ',') " +
                                                      "FROM EntradaNotaItem EI " +
                                                      "INNER JOIN Produto PR ON PR.Id = EI.IdProduto " +
                                                      "INNER JOIN UnidadeMedida UM ON UM.Id = PR.IdUnidadeMedida " +
                                                      "WHERE EI.IdEntradaNota = @IdEntradaNota ", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@IdEntradaNota", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                while (reader.Read()){

                    ret.Add(new EntradaNotaItemViewModel() {

                        IdNotaItem = (int) reader["IdNotaItem"],
                        IdProduto = (int)reader["IdProduto"],
                        QuantidadeProduto = (decimal) reader["QuantidadeProduto"],
                        NomeProduto = (string) reader["NomeProduto"],
                        UnidadeMedida = (string) reader["UnidadeMedida"],
                        ValorUnitarioProduto = (string) reader["ValorUnitarioProduto"],
                        ValorTotalProduto = (string) reader["ValorTotalProduto"]

                    });                
                }
            }
            return ret;
        }
    }
}
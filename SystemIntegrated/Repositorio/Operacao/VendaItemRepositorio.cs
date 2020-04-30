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
    public class VendaItemRepositorio
    {

        private SqlConnection con;

        public void Connection()
        {

            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public int SalvarItens(VendaItemModel vendaItemModel, int idVendaItem)
        {

            var ret = 0;

            Connection();


            using (SqlCommand command = new SqlCommand("INSERT INTO VendaProdutoItem ( IdVendaProduto,        " +
                                                      "                                IdProduto,             " +
                                                      "                                QuantidadeProduto,     " +
                                                      "                                ValorTotalProduto,     " +
                                                      "                                ValorDescontoProduto,  " +
                                                      "                                ValorUnitarioProduto   " +
                                                      "                             )                         " +
                                                      "                      VALUES( @IdVendaProduto,         " +
                                                      "                              @IdProduto,              " +
                                                      "                              @QuantidadeProduto,      " +
                                                      "                              @ValorTotalProduto,      " +
                                                      "                              @ValorDescontoProduto,   " +
                                                      "                              @ValorUnitarioProduto    " +
                                                      "                            )                          ", con))
            {
                con.Open();


                command.Parameters.AddWithValue("@IdVendaProduto", SqlDbType.Int).Value = idVendaItem;
                command.Parameters.AddWithValue("@IdProduto", SqlDbType.Int).Value = vendaItemModel.IdProduto;
                command.Parameters.AddWithValue("@QuantidadeProduto", SqlDbType.VarChar).Value = vendaItemModel.QuantidadeProduto;
                command.Parameters.AddWithValue("#ValorDescontoProduto", SqlDbType.VarChar).Value = vendaItemModel.ValorDescontoProduto;
                command.Parameters.AddWithValue("@ValorTotalProduto", SqlDbType.VarChar).Value = vendaItemModel.ValorTotalProduto;
                command.Parameters.AddWithValue("@ValorDescontoProduto", SqlDbType.VarChar).Value = vendaItemModel.ValorDescontoProduto;
                command.Parameters.AddWithValue("@ValorUnitarioProduto", SqlDbType.Int).Value = vendaItemModel.ValorUnitarioProduto;

                command.ExecuteScalar();

            }
            return ret;
        }

        public int AtualizarQuantidadeProduto(VendaItemModel vendaItemModel)
        {
            var ret = 0;

            Connection();

            using (SqlCommand command = new SqlCommand(" UPDATE Produto                                     " +
                                                       "    SET QuantEstoque = QuantEstoque + @QuantEstoque " +
                                                       "  WHERE Id = @Id                                    ", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@QuantEstoque", SqlDbType.Int).Value = vendaItemModel.QuantidadeProduto;
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = vendaItemModel.IdProduto;

                command.ExecuteNonQuery();
            }


            return ret;
        }

        public List<VendaItemViewModel> RecuperarPeloId(int id)
        {
            var ret = new List<VendaItemViewModel>();

            Connection();

            using (SqlCommand command = new SqlCommand("     SELECT VP.Id,                                                              " +
                                                       "            IdProduto = VP.IdProduto,                                           " +
                                                       "            NomeProduto = PR.Nome,                                              " +
                                                       "            UnidadeMedida = UM.Sigla,                                           " +
                                                       "            QuantidadeProduto = VP.QuantidadeProduto,                           " +
                                                       "            ValorUnitarioProduto = REPLACE(VP.ValorUnitarioProduto, '.', ','),  " +
                                                       "            ValorDescontoProduto = REPLACE(VP.ValorDescontoProduto, '.',','),   " +
                                                       "            ValorTotalProduto = REPLACE(VP.ValorTotalProduto, '.', ',')         " +
                                                       "       FROM VendaProdutoItem VP                                                 " +
                                                       " INNER JOIN Produto PR ON PR.Id = EI.IdProduto                                  " +
                                                       " INNER JOIN UnidadeMedida UM ON UM.Id = PR.IdUnidadeMedida                      " +
                                                       "      WHERE VP.IdVendaProduto = @IdVendaProduto                                 ", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@IdVendaProduto", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    ret.Add(new VendaItemViewModel()
                    {

                        IdVendaItem = (int)reader["IdVendaItem"],
                        IdProduto = (int)reader["IdProduto"],
                        QuantidadeProduto = (decimal)reader["QuantidadeProduto"],
                        NomeProduto = (string)reader["NomeProduto"],
                        UnidadeMedida = (string)reader["UnidadeMedida"],
                        ValorUnitarioProduto = (string)reader["ValorUnitarioProduto"],
                        ValorTotalProduto = (string)reader["ValorTotalProduto"]

                    });
                }
            }
            return ret;
        }
    }
}
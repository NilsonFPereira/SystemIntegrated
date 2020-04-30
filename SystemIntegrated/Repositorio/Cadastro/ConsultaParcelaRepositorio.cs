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
    public class ConsultaParcelaRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();

            con = new SqlConnection(constr);
        }


        public List<ParcelaModel> RecuperarLista(int numeroNota, string cnpjCpf)
        {
            var ret = new List<ParcelaModel>();

            Connection();

            using(SqlCommand command = new SqlCommand("     SELECT CL.Nome, " +
                                                      "            CL.Logradouro," +
                                                      "            CL.Numero," +
                                                      "            CL.Bairro," +
                                                      "            CL.Celular, "+
                                                      "            VP.NumeroVenda, "+
                                                      "            VPP.NumeroParcela, "+
                                                      "            DataVencimento = ISNULL(CONVERT(VARCHAR(10), VPP.DataVencimento, 103), ''), " +
                                                      "            VPP.ValorParcela, " +
                                                      "            DataPagamento = ISNULL(CONVERT(VARCHAR(10), VPP.DataPagamento, 103), ''), " +
                                                      "            StatusPagamento = SPP.Nome " +
                                                      "       FROM VendaProdutoParcela VPP " +
                                                      " INNER JOIN VendaProduto VP ON VP.Id = VPP.IdVendaProduto " +
                                                      " INNER JOIN StatusPagamentoParcela SPP ON SPP.Id = VPP.IdStatusPagamentoParcela " +
                                                      " INNER JOIN Cliente CL ON CL.Id = VP.IdCliente "+
                                                      "      WHERE VP.NumeroVenda = @numeroVenda "+
                                                      " AND CL.CnpjCpf = @cnpjCpf ", con ))
            {

                
                command.Parameters.AddWithValue("@numeroVenda", SqlDbType.Int).Value = numeroNota;
                command.Parameters.AddWithValue("@cnpjCpf", SqlDbType.VarChar).Value = cnpjCpf;

                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new ParcelaModel()
                    {
                        Nome = (string) reader["Nome"],
                        Logradouro = (string) reader["Logradouro"],
                        Numero = (string) reader["Numero"],
                        Bairro = (string) reader["Bairro"],
                        Celular = (string) reader["Celular"],
                        NumeroVenda =(string) reader["NumeroVenda"],
                        NumeroParcela = (int)reader["NumeroParcela"],
                        DataVencimento = (string)reader["DataVencimento"],
                        ValorParcela = (decimal)reader["ValorParcela"],
                        DataPagamento = (string)reader["DataPagamento"],
                        StatusPagamento = (string)reader["StatusPagamento"]
                    });
                };
            }

            return ret;



        }
    }
}
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
    public class RecebimentoRepositorio
    {

        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<VendaClienteViewModel> RecuperarVendaClientePelaBusca(string dadosBusca)
        {
            var ret = new List<VendaClienteViewModel>();

            Connection();

            using (SqlCommand command = new SqlCommand("     SELECT VP.Id,                                                            " +
                                                       "             CL.CnpjCpf,                                                      " +
                                                       "             CL.Nome,                                                         " +
                                                       "             Telefone = ISNULL(CL.Telefone, ''),                              " +
                                                       "             Celular = ISNULL(CL.Celular, ''),                                " +
                                                       "             Email = ISNULL(CL.Email, ''),                                    " +
                                                       "             CL.IdTipoPessoa,                                                 " +
                                                       "             VP.NumeroVenda,                                                  " +
                                                       "             DataVenda = CONVERT(VARCHAR(10), VP.DataVenda, 103),             " +
                                                       "             ValorTotalNota = REPLACE(VP.ValorTotalNota, '.',','),            " +
                                                       "             ValorPago = REPLACE( VP.ValorPago, '.',',')                      " +
                                                       "        FROM VendaProduto VP                                                  " +
                                                       " INNER JOIN Cliente CL ON CL.Id = VP.IdCliente                                " +
                                                       "      WHERE ( CL.CnpjCpf = @Valor OR CL.Nome = @Valor OR CL.Email = @Valor )  ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Valor", SqlDbType.VarChar).Value = dadosBusca;

                var reader = command.ExecuteReader();

                while (reader.Read() )
                {

                    ret.Add(new VendaClienteViewModel()
                    {
                        Id = (int)reader["Id"],
                        CnpjCpf = (string)reader["CnpjCpf"],
                        Nome = (string)reader["Nome"],
                        Telefone = (string)reader["Telefone"],
                        Celular = (string)reader["Celular"],
                        Email = (string)reader["Email"],
                        NumeroVenda = (string)reader["NumeroVenda"],
                        DataVenda = (string)reader["DataVenda"],
                        ValorTotalNota = (string)reader["ValorTotalNota"],
                        ValorPago = (string)reader["ValorPago"]
                    });
                }
            }
            return ret;
        }

        public List<VendaParcelaViewModel> RecuperarParcelasVendas(int idVenda)
        {
            var ret = new List<VendaParcelaViewModel>();

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT IdParcela = Id,                                                   " +
                                                      "        IdVendaProduto,                                                   " +
                                                      "        NumeroParcela,                                                    " +
                                                      "        DataVencimento = CONVERT(VARCHAR(10), DataVencimento, 103),       " +
                                                      "        ValorParcela = REPLACE(ValorParcela, '.',','),                    " +
                                                      "        ValorAcrescimoParcela = REPLACE(ValorAcrescimoParcela, '.',','),  " +
                                                      "        ValorDescontoParcela = REPLACE( ValorDescontoParcela, '.',','),   " +
                                                      "        ValorTotalParcela = REPLACE(ValorTotalParcela, '.',',')           " +
                                                      "   FROM VendaProdutoParcela                                               " +
                                                      "  WHERE IdVendaProduto = @IdVenda                                         " +
                                                      " ORDER BY NumeroParcela                                                   ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@IdVenda", SqlDbType.Int).Value = idVenda;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    ret.Add(new VendaParcelaViewModel()
                    {
                        IdParcela = (int)reader["IdParcela"],
                        IdVendaProduto = (int)reader["IdVendaProduto"],
                        NumeroParcela = (int) reader["NumeroParcela"],
                        DataVencimento = (string) reader["DataVencimento"],
                        ValorParcela = (string) reader["ValorParcela"],
                        ValorAcrescimoParcela = (string) reader["ValorAcrescimoParcela"],
                        ValorDescontoParcela = (string) reader["ValorDescontoParcela"],
                        ValorTotalParcela = (string) reader["ValorTotalParcela"]
                    });
                }

            }

            return ret;
        }
    }
}
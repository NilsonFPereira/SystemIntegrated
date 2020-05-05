using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SystemIntegrated.Models.Operacao;

namespace SystemIntegrated.Repositorio.Operacao
{
    public class VendaRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<VendaViewModel> RecuperarLista()
        {
            var ret = new List<VendaViewModel>();

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT Id,                                                         " +
                                                      "        DataVenda = CONVERT(VARCHAR(10), DataVenda, 103),           " +
                                                      "        IdCliente,                                                  " +
                                                      "        NumeroVenda,                                                " +
                                                      "        ValorTotalNota = REPLACE(ValorTotalNota,'.',','),           " +
                                                      "        ValorDesconto = REPLACE(ValorDesconto,'.',','),             " +
                                                      "        IdFormaPagamento,                                           " +
                                                      "        ValorPago = REPLACE(ValorPago,'.',','),                     " +
                                                      "        ValorFrete = REPLACE(ValorFrete,'.',','),                   " +
                                                      "        ValorAcrescimo = REPLACE(ValorAcrescimo,'.',','),          " +
                                                      "        IdFretePorConta,                                            " +
                                                      "        ValorProduto = ISNULL(REPLACE(ValorProduto,'.',','), ''),   " +
                                                      "        DataCadastro = CONVERT(VARCHAR(10), DataCadastro, 103 )     " +
                                                      "   FROM VendaProduto                                                ", con))
            {

                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new VendaViewModel()
                    {
                        Id = (int) reader["Id"],
                        DataVenda = (string) reader["DataVenda"],
                        IdCliente = (int) reader["IdCliente"],
                        NumeroVenda = (string) reader["NumeroVenda"],
                        ValorTotalNota = (string) reader["ValorTotalNota"],
                        ValorDesconto = (string) reader["ValorDesconto"],
                        IdFormaPagamento = (int)reader["IdFormaPagamento"],
                        ValorPago = (string) reader["ValorPago"],
                        ValorFrete = (string) reader["ValorFrete"],
                        IdFretePorConta = (int) reader["IdFretePorConta"],
                        ValorProduto = (string) reader["ValorProduto"],
                        DataCadastro = (string) reader["DataCadastro"],
                        ValorAcrescimo = (string) reader["ValorAcrescimo"]

                    });
                }
            }
            return ret;
        }
        public int RecuperarQuantidade()
        {
            var ret = 0;
            Connection();

            using( SqlCommand command = new SqlCommand(" SELECT COUNT(*)     " +
                                                       "   FROM VendaProduto ", con ) )
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }

            return ret;
        }


        public int RecuperarNovoNumero()
        {
            var ret = 1;

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT TOP 1 NumeroVenda = NumeroVenda + 1" +
                                                      "   FROM VendaProduto         " +
                                                      " ORDER BY NumeroVenda DESC    ", con ))
            {
                con.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    ret = (int)reader["NumeroVenda"];
                }

            }
            return ret;
        }
        public VendaViewModel RecuperarPeloId(int id)
        {
            VendaViewModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand("     SELECT VP.Id,                                                    " +
                                                      "            DataVenda = CONVERT(VARCHAR(10), VP.DataVenda, 103 ),     " +
                                                      "            VP.IdCliente,                                             " +
                                                      "            NomeCliente = CL.Nome,                                    " +
                                                      "            VP.NumeroVenda,                                           " +
                                                      "            ValorTotalNota = REPLACE(VP.ValorTotalNota,'.',','),      " +
                                                      "            ValorDesconto = REPLACE(VP.ValorDesconto,'.',','),        " +
                                                      "            ValorAcrescimo = REPLACE(VP.ValorAcrescimo,'.',','),      " +
                                                      "            VP.IdFormaPagamento,                                      " +
                                                      "            ValorPago = REPLACE(VP.ValorPago,'.',','),                " +
                                                      "            ValorFrete = REPLACE(VP.ValorFrete,'.',','),              " +
                                                      "            VP.IdFretePorConta,                                       " +
                                                      "            ValorProduto = REPLACE(VP.ValorProduto,'.',','),          " +
                                                      "            DataCadastro = CONVERT(VARCHAR(10), VP.DataCadastro, 103) " +
                                                      "       FROM VendaProduto VP                                           " +
                                                      " INNER JOIN Cliente CL ON CL.Id = VP.IdCliente                        " +
                                                      "      WHERE VP.Id=@Id                                                    ", con ) )
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new VendaViewModel()
                    {
                        Id = (int) reader["Id"],
                        DataVenda = (string) reader["DataVenda"],
                        NumeroVenda = (string)reader["NumeroVenda"],
                        IdCliente = (int) reader["IdCliente"],
                        NomeCliente = (string) reader["NomeCliente"],
                        IdFretePorConta = (int) reader["IdFretePorConta"],
                        ValorFrete = (string) reader["ValorFrete"],
                        ValorProduto = (string) reader["ValorProduto"],
                        ValorDesconto = (string) reader["ValorDesconto"],
                        ValorAcrescimo = (string) reader["ValorAcrescimo"],
                        ValorTotalNota = (string) reader["ValorTotalNota"],
                        ValorPago = (string) reader["ValorPago"],
                        IdFormaPagamento = (int) reader["IdFormaPagamento"]



                    };

                }
            }
            return ret;
        }


        public int Salvar(VendaModel vendaModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(vendaModel.Id);

            if(model == null)
            {
                Connection();

                using(SqlCommand command = new SqlCommand("INSERT INTO VendaProduto ( DataVenda,          " +
                                                          "                           IdCliente,          " +
                                                          "                           NumeroVenda,        " +
                                                          "                           ValorTotalNota,     " +
                                                          "                           ValorDesconto,      " +
                                                          "                           IdFormaPagamento,   " +
                                                          "                           ValorPago,          " +
                                                          "                           ValorFrete,         " +
                                                          "                           IdFretePorConta,    " +
                                                          "                           ValorProduto,       " +
                                                          "                           DataCadastro,       " +
                                                          "                           ValorAcrescimo      " +
                                                          "                         )                     " +
                                                          "                  VALUES ( @DataVenda,         " +
                                                          "                           @IdCliente,         " +
                                                          "                           @NumeroVenda,       " +
                                                          "                           @ValorTotalNota,    " +
                                                          "                           @ValorDesconto,     " +
                                                          "                           @IdFormaPagamento,  " +
                                                          "                           @ValorPago,         " +
                                                          "                           @ValorFrete,        " +
                                                          "                           @IdFretePorConta,   " +
                                                          "                           @ValorProduto,      " +
                                                          "                           @DataCadastro,      " +
                                                          "                           @ValorAcrescimo     " +
                                                          "                         );                    " +
                                                          " select convert(int, scope_identity())", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@DataVenda", SqlDbType.VarChar).Value = vendaModel.DataVenda;
                    command.Parameters.AddWithValue("@IdCliente", SqlDbType.VarChar).Value = vendaModel.IdCliente;
                    command.Parameters.AddWithValue("@NumeroVenda", SqlDbType.VarChar).Value = vendaModel.NumeroVenda;
                    command.Parameters.AddWithValue("@ValorTotalNota", SqlDbType.Decimal).Value = vendaModel.ValorTotalNota;
                    command.Parameters.AddWithValue("@ValorDesconto", SqlDbType.Decimal).Value = vendaModel.ValorDesconto;
                    command.Parameters.AddWithValue("@IdFormaPagamento ", SqlDbType.Int).Value = vendaModel.IdFormaPagamento;
                    command.Parameters.AddWithValue("@ValorPago", SqlDbType.Decimal).Value = vendaModel.ValorPago;
                    command.Parameters.AddWithValue("@ValorFrete", SqlDbType.Decimal).Value = vendaModel.ValorFrete;
                    command.Parameters.AddWithValue("@IdFretePorConta", SqlDbType.Int).Value = vendaModel.IdFretePorConta;
                    command.Parameters.AddWithValue("@ValorProduto", SqlDbType.Decimal).Value = vendaModel.ValorProduto;
                    command.Parameters.AddWithValue("@DataCadastro", SqlDbType.VarChar).Value = vendaModel.DataCadastro;
                    command.Parameters.AddWithValue("@ValorAcrescimo", SqlDbType.Decimal).Value = vendaModel.ValorAcrescimo;
                    ret = (int)command.ExecuteScalar();

                }
            }
            return ret;
        }
    }
}
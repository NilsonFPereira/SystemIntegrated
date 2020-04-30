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
    public class ConsultaVendaPorClienteRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();

            con = new SqlConnection(constr);

        }

        public List<VendasPorClienteModel> RecuperarLista(string cnpjcpf)
        {
            var ret = new List<VendasPorClienteModel>();

            Connection();

            using(SqlCommand command = new SqlCommand("     SELECT CL.CnpjCpf," +
                                                      "            IdVendaProduto = VP.Id,                    " +
                                                      "     DataVenda = CONVERT(VARCHAR(10), DataVenda, 103), "+
                                                      "            NumeroVenda,                               " +
                                                      "            ValorTotalNota                             " +                                                    
                                                      "       FROM VendaProduto VP                            " +
                                                      " INNER JOIN Cliente CL ON CL.Id = VP.IdCliente " +
                                                      " WHERE 1 = 1", con))
            {

                con.Open();
                command.Parameters.AddWithValue("@cnpjcpf", SqlDbType.VarChar).Value = cnpjcpf;

                var reader = command.ExecuteReader();

                while (reader.Read()){

                    ret.Add(new VendasPorClienteModel()
                    {
                        CnpjCpf = (string) reader["CnpjCpf"],
                        IdVendaProduto = (int) reader["IdVendaProduto"],
                        DataVenda = (string)reader["DataVenda"],
                        NumeroVenda = (string) reader["NumeroVenda"],
                        ValorTotalNota = (decimal) reader["ValorTotalNota"]
             
                    });

                }

            }
            return ret;


        }

    }
}
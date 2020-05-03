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
    public class VendaParcelaRepositorio
    {

        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<VendaParcelaViewModel> RecuperarVendaParcelas(int id)
        {

            var ret = new List<VendaParcelaViewModel>();

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT Parcela = CAST(NumeroParcela AS VARCHAR) +' - '+ CONVERT(VARCHAR(10), DataVencimento, 103) +' - '+ REPLACE(ValorTotalParcela, '.',',')  " +
                                                      "   FROM VendaProdutoParcela WHERE IdVendaProduto = @Id", con))
            {
                con.Open();
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new VendaParcelaViewModel()
                    {
                        Parcela = (string)reader["Parcela"]

                    });
                }
            }

            return ret;

        }

    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemIntegrated.Models.Cadastro;

namespace SystemIntegrated.Repositorio.Cadastro
{
    public class FormaPagamentoRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public List<FormaPagamentoViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<FormaPagamentoViewModel>();

            Connection();

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());
            }

            var pos = (pagina - 1) * tamPag;
            var paginacao = "";

            if (pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }


            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *             " +
                                                                    "     FROM FormaPagamento " +
                                                                               filtroWhere      +
                                                                    " ORDER BY Codigo          " +
                                                                               paginacao
                                                                    ), con))
            {
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new FormaPagamentoViewModel()
                    {

                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"]

                    });

                }
                return ret;

            }

        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT COUNT(*)       " +
                                                      "   FROM FormaPagamento ", con ) )
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;

        }

        public FormaPagamentoModel RecuperarPeloId(int id)
        {
            FormaPagamentoModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand("SELECT * FROM FormaPagamento WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    ret = new FormaPagamentoModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"]

                    };
                }
            }

            return ret;
        }

        public int Salvar(FormaPagamentoModel formaPagamentoModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(formaPagamentoModel.Id);

            if( model == null )
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" INSERT INTO FormaPagamento( Codigo,   " +
                                                          "                             Nome      " +
                                                          "                           )           " +
                                                          "                     VALUES( @Codigo,  " +
                                                          "                             @Nome     " +
                                                          "                            );         " +
                                                          " select convert(int, scope_identity()) ", con ) )
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = formaPagamentoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = formaPagamentoModel.Nome;

                    ret = (int)command.ExecuteScalar();

                }
            }else
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" UPDATE FormaPagamento  " +
                                                          "    SET Codigo=@Codigo, " +
                                                          "        Nome=@Nome      " +
                                                          "  WHERE Id=@Id", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = formaPagamentoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = formaPagamentoModel.Nome;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = formaPagamentoModel.Id;
                
                    if(command.ExecuteNonQuery() > 0)
                    {

                        ret = formaPagamentoModel.Id;

                    }
                }
            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using(SqlCommand command = new SqlCommand(" DELETE FormaPagamento " +
                                                      "  WHERE Id=@Id         ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (command.ExecuteNonQuery() > 0);

            }

            return ret;

        }
    }
}
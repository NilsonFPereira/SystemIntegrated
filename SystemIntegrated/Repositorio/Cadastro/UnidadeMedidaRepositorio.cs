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
    public class UnidadeMedidaRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<UnidadeMedidaViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<UnidadeMedidaViewModel>();
            Connection();

            var paginacao = "";
            var pos = (pagina - 1) * tamPag;

            if (pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {
                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }

            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *                " +
                                                                     "     FROM UnidadeMedida    " +
                                                                                filtroWhere      +
                                                                     " ORDER BY Nome             " +
                                                                                paginacao), con))
            {
                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new UnidadeMedidaViewModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Sigla = (string)reader["Sigla"],
                        Ativo = (bool) reader["Ativo"]


                    });
                }
                return ret;
            }
        }

        public UnidadeMedidaModel RecuperarPeloId(int id)
        {
            UnidadeMedidaModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT *            " +
                                                      "  FROM UnidadeMedida " +
                                                      " WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new UnidadeMedidaModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Sigla = (string)reader["Sigla"],
                        Ativo = (bool) reader["Ativo"]

                    };
                }
            }
            return ret;
        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*)" +
                                                      "  FROM UnidadeMedida", con))
            {

                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {

            var ret = false;

            Connection();

            using (SqlCommand command = new SqlCommand(" DELETE UnidadeMedida" +
                                                      "  WHERE Id=@Id", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar(UnidadeMedidaModel unidadeMedidaModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(unidadeMedidaModel.Id);

            if (model == null)
            {
                Connection();

                using (SqlCommand command = new SqlCommand("    INSERT INTO UnidadeMedida ( Nome,        " +
                                                           "                                Sigla,       " +
                                                           "                                Ativo        " +
                                                           "                              )              " +
                                                           "                       VALUES ( @Nome,       " +
                                                           "                                @Sigla,      " +
                                                           "                                @Ativo       " +
                                                           "                         );                  " +
                                                           " select convert(int, scope_identity())       ", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = unidadeMedidaModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = unidadeMedidaModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = unidadeMedidaModel.Ativo;
                    ret = (int)command.ExecuteScalar();
                }
            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand("UPDATE UnidadeMedida       " +
                                                           "   SET Nome=@Nome,         " +
                                                           "       Sigla=@Sigla,       " +
                                                           "       Ativo=@Ativo        " +
                                                           " WHERE Id=@Id              ", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = unidadeMedidaModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.Int).Value = unidadeMedidaModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = unidadeMedidaModel.Ativo;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = unidadeMedidaModel.Id;

                    if (command.ExecuteNonQuery() > 0)
                    {

                        ret = unidadeMedidaModel.Id;
                    }
                }
            }
            return ret;
        }
    }
}
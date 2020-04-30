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
    public class NaturezaRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<NaturezaViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<NaturezaViewModel>();
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

            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *           " +
                                                                     "     FROM Natureza    " +
                                                                                filtroWhere   +
                                                                     " ORDER BY Codigo DESC  " +
                                                                                paginacao), con))
            {
                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new NaturezaViewModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"]
                    });
                }
                return ret;
            }
        }
       
        public NaturezaModel RecuperarPeloId(int id)
        {
                NaturezaModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT *            " +
                                                      "  FROM Natureza " +
                                                      " WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new NaturezaModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string) reader["Codigo"],
                        Nome = (string)reader["Nome"]

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
                                                      "  FROM Natureza", con))
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

            using (SqlCommand command = new SqlCommand(" DELETE Natureza" +
                                                      "  WHERE Id=@Id", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar(NaturezaModel naturezaModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(naturezaModel.Id);

            if (model == null)
            {
                Connection();

                using (SqlCommand command = new SqlCommand("    INSERT INTO Natureza ( Nome,        " +
                                                           "                           Codigo       " +
                                                           "                         )              " +
                                                           "                  VALUES ( @Nome,       " +
                                                           "                           @Codigo      " +
                                                           "                         );             " +
                                                           " select convert(int, scope_identity())  ", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = naturezaModel.Nome;
                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = naturezaModel.Codigo;

                    ret = (int)command.ExecuteScalar();
                }
            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand("UPDATE Natureza       " +
                                                           "   SET Nome=@Nome,    " +
                                                           "       Codigo=@Codigo " +
                                                           " WHERE Id=@Id         ", con ) )
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = naturezaModel.Nome;
                    command.Parameters.AddWithValue("@Codigo", SqlDbType.Int).Value = naturezaModel.Codigo;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = naturezaModel.Id;

                    if (command.ExecuteNonQuery() > 0)
                    {

                        ret = naturezaModel.Id;
                    }
                }
            }
            return ret;
        }
    }
}
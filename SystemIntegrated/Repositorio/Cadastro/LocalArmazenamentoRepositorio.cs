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
    public class LocalArmazenamentoRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {

            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public List<LocalArmazenamentoViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<LocalArmazenamentoViewModel>();

            Connection();
            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }

            var paginacao = "";

            var pos = (pagina - 1) * tamPag;

            if (pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }

            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *                " +
                                                                    "     FROM LocalArmazenamento " +
                                                                               filtroWhere +
                                                                    " ORDER BY Nome             " +
                                                                               paginacao
                                                                    ), con))
            {
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new LocalArmazenamentoViewModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Ativo = (bool)reader["Ativo"]

                    });
                }
                return ret;
            }
        }
        public LocalArmazenamentoModel RecuperarPeloId(int id)
        {
            LocalArmazenamentoModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand(" SELECT *                " +
                                                      "   FROM LocalArmazenamento " +
                                                      "  WHERE Id=@Id           ", con))
            {
                con.Open();
                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new LocalArmazenamentoModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Ativo = (bool)reader["Ativo"]

                    };
                }
            }
            return ret;
        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using (SqlCommand command = new SqlCommand(" SELECT COUNT(*)" +
                                                      "   FROM LocalArmazenamento", con))
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

            using (SqlCommand command = new SqlCommand(" DELETE LocalArmazenamento" +
                                                      "  WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;
            }

            return ret;
        }

        public int Salvar(LocalArmazenamentoModel localArmazenamentoModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(localArmazenamentoModel.Id);

            if (model == null)
            {
                Connection();

                using (SqlCommand command = new SqlCommand("INSERT INTO LocalArmazenamento ( Nome, " +
                                                          "                               Ativo " +
                                                          "                             )" +
                                                          "                      VALUES ( @Nome, " +
                                                          "                               @Ativo);" +
                                                          " select convert( int , scope_identity())", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = localArmazenamentoModel.Nome;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = localArmazenamentoModel.Ativo;

                    ret = (int)command.ExecuteScalar();

                }

            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand(" UPDATE LocalArmazenamento" +
                                                          "    SET Nome=@Nome," +
                                                          "        Ativo=@Ativo" +
                                                          "  WHERE Id=@Id ", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = localArmazenamentoModel.Nome;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = localArmazenamentoModel.Ativo;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = localArmazenamentoModel.Id;

                    if (command.ExecuteNonQuery() > 0)
                    {

                        ret = localArmazenamentoModel.Id;

                    }
                }
            }

            return ret;
        }
    }
}
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
    public class PaisRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Pais", con))
            {

                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;
        }

        public List<PaisViewModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "")
        {
            var ret = new List<PaisViewModel>();

            Connection();

            var filtroWhere = "";

            if(! string.IsNullOrEmpty(filtro ) )
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }

            var pos = (pagina - 1) * tamPagina;
            var paginacao = "";


            if (pagina > 0 && tamPagina > 0)
            {

                paginacao = string.Format(" offset {0} rows fetch next {1} rows only", pos, tamPagina);

            }

            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *    " +
                                                                     "     FROM Pais " +
                                                                     filtroWhere +
                                                                     " ORDER BY Nome   " +
                                                                     paginacao ), con))
            {
               
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new PaisViewModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["NoME"],
                        Ativo = (bool)reader["Ativo"],
                        Sigla = (string)reader["Sigla"],
                    });
                }

            }
            return ret;
        }

        public PaisModel RecuperarPeloId(int id)
        {
            PaisModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Pais WHERE Id= @id", con))
            {
                con.Open();
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();


                if (reader.Read())
                {

                    ret = new PaisModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"],
                        Sigla = (string)reader["Sigla"],
                        Ativo = (bool)reader["Ativo"]
                    };

                }
            }
            return ret;

        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using (SqlCommand command = new SqlCommand("DELETE Pais WHERE Id = @id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                ret = (command.ExecuteNonQuery() > 0);

            }
            return ret;
        }

        public int Salvar(PaisModel paisModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(paisModel.Id);

            if (model == null)
            {

                Connection();

                using (SqlCommand command = new SqlCommand(" INSERT INTO Pais ( Codigo,           " +
                                                           "                    Nome,             " +
                                                           "                    Sigla,            " +
                                                           "                    Ativo             " +
                                                           "                  )                   " +
                                                           "           VALUES ( @Codigo,          " +
                                                           "                    @Nome,            " +
                                                           "                    @Sigla,           " +
                                                           "                    @Ativo            " +
                                                           "                  );                  " +
                                                           " select convert(int, scope_identity())", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = paisModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = paisModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = paisModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = paisModel.Ativo;

                    ret = (int)command.ExecuteScalar();

                }

            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand(" UPDATE Pais            " +
                                                           "    SET Codigo=@codigo, " +
                                                           "        Nome=@nome,     " +
                                                           "        Sigla=@Sigla,   " +
                                                           "        Ativo=@ativo    " +
                                                           "  WHERE Id=@id          ", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = paisModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = paisModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = paisModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.VarChar).Value = (paisModel.Ativo ? 1 : 0);
                    command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = paisModel.Id;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        ret = paisModel.Id;

                    }
                }
            }
            return ret;
        }
    }
}
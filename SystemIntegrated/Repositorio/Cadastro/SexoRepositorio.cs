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
    public class SexoRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public List<SexoViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "" )
        {

            var ret = new List<SexoViewModel>();

            Connection();

            var filtroWhere = "";

            if ( ! string.IsNullOrEmpty(filtro))
            {
                filtroWhere = string.Format(" WHERE LOWER(NOME) LIKE '%{0}%'", filtro.ToLower());


            }

            var paginacao = "";

            var pos = (pagina - 1) * tamPag;

            if ( pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROW ONLY", pos, tamPag);

            }

            using(SqlCommand command = new SqlCommand(string.Format("   SELECT *          " +
                                                                    "     FROM Sexo       " +
                                                                               filtroWhere  +
                                                                    " ORDER BY Id       " +
                                                                               paginacao 
                                                                   ), con ) )
            {
                con.Open();

                var reader = command.ExecuteReader();


                while (reader.Read())
                {

                    ret.Add(new SexoViewModel()
                    {
                        Id = (int) reader["Id"],
                        Nome = (string)reader["Nome"],
                        Sigla = (string)reader["Sigla"],
                        Ativo = (bool)reader["Ativo"]

                    });
                }
                return ret;
            }
        }


        public SexoModel RecuperarPeloId(int id)
        {
           

            SexoModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Sexo WHERE Id = @id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new SexoModel()
                    {
                        Id = (int) reader["Id"],
                        Nome = (string)reader["Nome"],
                        Sigla = (string)reader["Sigla"],
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

            using(SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Sexo", con))
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
            using( SqlCommand command = new SqlCommand(" DELETE Sexo   " +
                                                       "  WHERE Id=@id ", con ))
            {
                con.Open();
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                ret = (int) command.ExecuteNonQuery() > 0;

            }

            return ret;


        }

        public int Salvar(SexoModel sexoModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(sexoModel.Id);

            if(model == null)
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" INSERT INTO Sexo ( Nome,                 " +
                                                          "                    Sigla,                " +
                                                          "                    Ativo                 " +
                                                          "                  )                       " +
                                                          "           VALUES ( @Nome,                " +
                                                          "                    @Sigla,               " +
                                                          "                    @Ativo                " +
                                                          "                  );                      " +
                                                          "    select convert( int, scope_identity() " +
                                                          "                  )", con ))
                {

                    con.Open();
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = sexoModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = sexoModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = sexoModel.Ativo;

                    ret = (int) command.ExecuteScalar();

                }


            }else
            {

                Connection();

                using(SqlCommand command = new SqlCommand(" UPDATE Sexo" +
                                                          "    SET Nome=@Nome," +
                                                          "        Sigla=@Sigla," +
                                                          "        Ativo=@Ativo" +
                                                          "  WHERE Id=@Id", con ))
                {
                    con.Open();
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = sexoModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = sexoModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = sexoModel.Ativo;
                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = sexoModel.Id;

                    if(command.ExecuteNonQuery() > 0)
                    {

                        ret = sexoModel.Id;

                    }

                }

            }




            return ret;

        }


    }
}
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
    public class NivelUsuarioRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<NivelUsuarioViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<NivelUsuarioViewModel>();

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

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%' OR LOWER(Codigo) LIKE '%{1}%'", filtro.ToLower(), filtro.ToLower());

            }


            using (SqlCommand command = new SqlCommand(string.Format("   SELECT *            " +
                                                                    "     FROM NivelUsuario " +
                                                                               filtroWhere +
                                                                    " ORDER BY Codigo DESC  " +
                                                                               paginacao
                                                                    ), con))
            {
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new NivelUsuarioViewModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"],
                        Ativo = (bool)reader["Ativo"]

                    });
                }
                return ret;
            }
        }
        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*)     " +
                                                      "  FROM NivelUsuario ", con))
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;
        }

        public NivelUsuarioModel RecuperarPeloId(int id)
        {
            NivelUsuarioModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand(" SELECT *            " +
                                                      "   FROM NivelUsuario " +
                                                      "  WHERE Id=@Id       ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new NivelUsuarioModel()
                    {
                        Id = (int)reader["Id"],
                        Codigo = (string)reader["Codigo"],
                        Nome = (string)reader["Nome"],
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
            using (SqlCommand commandExcluiNivel_Usuario = new SqlCommand(" DELETE FROM Nivel_Usuario " +
                                                                         "  WHERE IdNivelUsuario=@IdNivelUsuario", con))
            {
                con.Open();

                commandExcluiNivel_Usuario.Parameters.AddWithValue("@IdNivelUsuario", SqlDbType.Int).Value = id;
                commandExcluiNivel_Usuario.ExecuteScalar();

                con.Close();

            }

            using (SqlCommand command = new SqlCommand(" DELETE NivelUsuario " +
                                                      "  WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar(NivelUsuarioModel nivelUsuarioModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(nivelUsuarioModel.Id);

                if (model == null)
                {
                    Connection();

                    using (SqlCommand command = new SqlCommand("INSERT INTO NivelUsuario ( Codigo,    " +
                                                              "                           Nome,      " +
                                                              "                           Ativo      " +
                                                              "                         )            " +
                                                              "                  VALUES ( @Codigo,   " +
                                                              "                           @Nome,     " +
                                                              "                           @Ativo     " +
                                                              "                         );           " +
                                                              " select convert(int, scope_identity()) ", con))
                    {

                        con.Open();

                        command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = nivelUsuarioModel.Codigo;
                        command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = nivelUsuarioModel.Nome;
                        command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = nivelUsuarioModel.Ativo;

                    nivelUsuarioModel.Id = (int)command.ExecuteScalar();

                    ret = nivelUsuarioModel.Id;

                }

            }
                else
                {
                    Connection();


                    using (SqlCommand command = new SqlCommand(" UPDATE NivelUsuario    " +
                                                              "    SET Codigo=@Codigo, " +
                                                              "        Nome=@Nome,     " +
                                                              "        Ativo=@Ativo    " +
                                                              "  WHERE Id=@Id          ", con))
                    {
                        con.Open();
                        
                        command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = nivelUsuarioModel.Codigo;
                        command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = nivelUsuarioModel.Nome;
                        command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = nivelUsuarioModel.Ativo;
                        command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = nivelUsuarioModel.Id;

                        if (command.ExecuteNonQuery() > 0)
                        {

                            ret = nivelUsuarioModel.Id;
                        }
                    }
                }

                if (nivelUsuarioModel.Usuarios != null && nivelUsuarioModel.Usuarios.Count > 0)
                {

                    Connection();
    
                    using (SqlCommand commandExclusaoNivelUsuario = new SqlCommand(" DELETE Nivel_Usuario               " +
                                                                                       "  WHERE IdNivelUsuario = @IdNivelUsuario ", con))
                    {
                        con.Open();
                        
                        commandExclusaoNivelUsuario.Parameters.AddWithValue("@IdNivelUsuario", SqlDbType.Int).Value = nivelUsuarioModel.Id;

                        commandExclusaoNivelUsuario.ExecuteScalar();

                    }

                    foreach (var usuario in nivelUsuarioModel.Usuarios)
                    {
                    Connection();
                        using (SqlCommand commandInclusaoNivelUsuario = new SqlCommand(" INSERT INTO Nivel_Usuario( IdNivelUsuario,  " +
                                                                                      "                            IdUsuario        " +
                                                                                      "                          )                  " +
                                                                                      "                   VALUES ( @IdNivelUsuario, " +
                                                                                      "                            @IdUsuario       " +
                                                                                      "                          )                  ", con))
                        {
                            con.Open();
                            

                            commandInclusaoNivelUsuario.Parameters.AddWithValue("@IdNivelUsuario", SqlDbType.Int).Value = nivelUsuarioModel.Id;
                            commandInclusaoNivelUsuario.Parameters.AddWithValue("@IdUsuario", SqlDbType.Int).Value = usuario.Id;

                            commandInclusaoNivelUsuario.ExecuteScalar();
                        }

                    }

            }
            return ret;
        }
    }
}
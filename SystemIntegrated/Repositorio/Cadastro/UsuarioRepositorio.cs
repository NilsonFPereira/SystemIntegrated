using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemIntegrated.Helpers;
using SystemIntegrated.Models;

namespace SystemIntegrated.Repositorio
{
    public class UsuarioRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            String constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<UsuarioViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<UsuarioViewModel>();

            Connection();

            var paginacao = "";

            var pos = (pagina - 1) * tamPag;

            if(pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);
            }

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {
                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%' OR LOWER(Usuario) LIKE '%{1}%'", filtro.ToLower(), filtro.ToLower());

            }
            using (SqlCommand command = new SqlCommand( string.Format(" SELECT Id,                                                     " +
                                                       "                       Nome,                                                   " +
                                                       "                       Usuario,                                                " +
                                                       "                       Email,                                                  " +
                                                       "                       Cpf,                                                    " +
                                                       "                       DataCadastro = CONVERT(VARCHAR(10), DataCadastro, 103), " +
                                                       "                       Ativo                                                   " +
                                                       "                  FROM Usuario                                                 " +
                                                                               filtroWhere                                               +
                                                       "              ORDER BY Nome                                                    " +
                                                                               paginacao ), con ) )
            {

                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new UsuarioViewModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Usuario = (string) reader["Usuario"],
                        Email = (string)reader["Email"],
                        Cpf = (string)reader["Cpf"],
                        DataCadastro = (string)reader["DataCadastro"],
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

            using(SqlCommand command = new SqlCommand(" SELECT COUNT(*) " +
                                                      "   FROM Usuario  ", con ) )
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;
        }

        public UsuarioModel ValidarUsuario(string login, string senha)
        {
            Connection();
            UsuarioModel ret = null;

            using (SqlCommand command = new SqlCommand("SELECT *                   " +
                                                       "  FROM Usuario             " +
                                                       " WHERE (    Nome=@login    " +
                                                       "         OR Usuario=@login " +
                                                       "         OR Cpf=@login     " +
                                                       "         OR Email=@login   " +
                                                       "       )                   " +
                                                       "   AND (                   " +
                                                       "            Senha=@senha   " +
                                                       "        )                  ", con ) )
            {
                con.Open();

                command.Parameters.AddWithValue("@login", SqlDbType.VarChar).Value = login;
                command.Parameters.AddWithValue("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    ret = new UsuarioModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"],
                        Usuario = (string)reader["Usuario"],
                        Email = (string)reader["Email"],
                        Cpf = (string)reader["Cpf"]

                    };
                }

            }
            return ret;
        }

        public UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT Id," +
                                                      "        Nome, " +
                                                      "        Usuario, " +
                                                      "        Email, " +
                                                      "        Cpf, " +
                                                      "        Senha, " +
                                                      "        DataCadastro = CONVERT(VARCHAR(10), DataCadastro, 103), " +
                                                      "        DataVencimento = CONVERT(VARCHAR(10), DataVencimento, 103) ," +
                                                      "        Ativo" +
                                                      "   FROM Usuario  " +
                                                      "  WHERE Id=@Id   ", con ) )
            {

                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ret = new UsuarioModel()
                    {
                        Id             = (int)      reader["Id"],
                        Nome           = (string)   reader["Nome"],
                        Usuario        = (string)   reader["Usuario"],
                        Email          = (string)   reader["Email"],
                        Cpf            = (string)   reader["Cpf"],
                        Senha          = (string)   reader["Senha"],
                        DataCadastro   = (string) reader["DataCadastro"],
                        DataVencimento = (string) reader["DataVencimento"],
                        Ativo          = (bool)     reader["Ativo"]
                    };                
                }
                return ret;
            }
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using(SqlCommand command = new SqlCommand(" DELETE Usuario " +
                                                      "  WHERE Id=@Id  ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;


                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar(UsuarioModel usuarioModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(usuarioModel.Id);

            if(model == null)
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" INSERT INTO Usuario( Nome,            " +
                                                          "                      Usuario,         " +
                                                          "                      Cpf,             " +
                                                          "                      Email,           " +
                                                          "                      Senha,           " +
                                                          "                      DataCadastro,    " +
                                                          "                      DataVencimento,  " +
                                                          "                      Ativo            " +
                                                          "                    )                  " +
                                                          "             VALUES ( @Nome,           " +
                                                          "                      @Usuario,        " +
                                                          "                      @Cpf,            " +
                                                          "                      @Email,          " +
                                                          "                      @Senha,          " +
                                                          "                      @DataCadastro,   " +
                                                          "                      @DataVencimento,  " +
                                                          "                      @Ativo           " +
                                                          "                    );                 " +
                                                          " select convert(int, scope_identity()) ", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Nome",           SqlDbType.VarChar ).Value = usuarioModel.Nome;
                    command.Parameters.AddWithValue("@Usuario",        SqlDbType.VarChar ).Value = usuarioModel.Usuario;
                    command.Parameters.AddWithValue("@Cpf",            SqlDbType.VarChar ).Value = usuarioModel.Cpf;
                    command.Parameters.AddWithValue("@Email",          SqlDbType.VarChar ).Value = usuarioModel.Email;
                    command.Parameters.AddWithValue("@Senha",          SqlDbType.VarChar ).Value = CriptoHelper.HashMD5(usuarioModel.Senha );
                    command.Parameters.AddWithValue("@DataCadastro",   SqlDbType.DateTime).Value = usuarioModel.DataCadastro;
                    command.Parameters.AddWithValue("@DataVencimento", SqlDbType.DateTime).Value = usuarioModel.DataVencimento;
                    command.Parameters.AddWithValue("@Ativo",          SqlDbType.Int     ).Value = usuarioModel.Ativo;

                    ret = (int)command.ExecuteScalar();

                }

            }
            else
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" UPDATE Usuario                         " +
                                                          "    SET Nome=@Nome,                     " +
                                                          "        Usuario=@Usuario,               " +
                                                          "        Cpf=@Cpf,                       " +
                                                          "        Email=@Email,                   " +
                                                          "        Ativo=@Ativo                    " +
                                                          "  WHERE Id=@Id                          ", con ) )
                {
                    con.Open();
                   
                    command.Parameters.AddWithValue("@Nome",           SqlDbType.VarChar).Value = usuarioModel.Nome;
                    command.Parameters.AddWithValue("@Usuario",        SqlDbType.VarChar).Value = usuarioModel.Usuario;
                    command.Parameters.AddWithValue("@Cpf",            SqlDbType.VarChar).Value = usuarioModel.Cpf;
                    command.Parameters.AddWithValue("@Email",          SqlDbType.VarChar).Value = usuarioModel.Email;
                    command.Parameters.AddWithValue("@Ativo",          SqlDbType.Int).Value     = usuarioModel.Ativo;
                    command.Parameters.AddWithValue("@Id",             SqlDbType.Int).Value     = usuarioModel.Id;

                    if(command.ExecuteNonQuery() > 0)
                    {
                        ret = usuarioModel.Id;
                    }
                }
            }
            return ret;
        }
        public string RecuperarStringPerfil(int id)
        {
            var ret = string.Empty;

            Connection();

            using (SqlCommand command = new SqlCommand("     SELECT NI.Nome                                      " +
                                                       "       FROM Nivel_Usuario NU, NivelUsuario NI            " +
                                                       "     WHERE ( NU.IdUsuario = @IdUsuario )                 " +
                                                       "       AND ( NU.IdNivelUsuario = NI.Id )                 " +
                                                       "       AND ( NI.Ativo = 1 )", con) )
            {

                con.Open();
                command.Parameters.AddWithValue("@IdUsuario", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    ret += (ret != string.Empty ? ";" : string.Empty) + (string)reader["Nome"];

                }

            }
                return ret;
        }

    }
}
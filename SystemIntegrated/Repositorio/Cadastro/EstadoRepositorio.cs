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
    public class EstadoRepositorio
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

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Estado", con))
            {

                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;
        }


        public List<EstadoViewModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "")
        {

            var ret = new List<EstadoViewModel>();

            Connection();

            var filtroWhere = "";
            if ( ! string.IsNullOrEmpty(filtro))
            {
              filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro);

            }

            var pos = ( pagina - 1 ) * tamPagina;
            var paginacao = "";

            if(pagina > 0 && tamPagina > 0)
            {

                paginacao = string.Format(" offset {0} rows fetch next {1} rows only", pos, tamPagina);

            }
            
            using(SqlCommand command = new SqlCommand(string.Format("      SELECT Id,        " +
                                                                    "             Codigo,    " +
                                                                    "             Nome,      " +
                                                                    "             Sigla,     " +
                                                                    "             NomePais,  " +
                                                                    "             Ativo      " +
                                                                    "        FROM EstadoView " +
                                                                                  filtroWhere  +
                                                                    "    ORDER BY Nome       " +
                                                                                  paginacao ), con ) )
            {
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    ret.Add(new EstadoViewModel()
                    {
                        Id = (int) reader["Id"],
                        Codigo = (string) reader["Codigo"],
                        Nome = (string) reader["Nome"],
                        Sigla = (string) reader["Sigla"],
                        NomePais = (string) reader["NomePais"],
                        Ativo = (bool) reader["Ativo"]

                    });
                }

            }

            return ret;

        }
        public EstadoModel RecuperarPeloId(int id)
        {

            EstadoModel ret = null;

            Connection();

            using(SqlCommand command = new SqlCommand("     SELECT ES.Id,                       " +
                                                      "            ES.Codigo,                   " +
                                                      "            ES.Nome,                     " +
                                                      "            ES.Sigla,                    " +
                                                      "            ES.Ativo,                    " +
                                                      "            IdPais = PA.Id,              " +
                                                      "            NomePais = PA.Nome           " +
                                                      "       FROM Estado ES                    " +
                                                      " INNER JOIN Pais PA ON PA.Id = ES.IdPais " +
                                                      "      WHERE ES.Id = @id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if(reader.Read())
                {
                    ret = new EstadoModel()
                    {
                        Id = (int) reader["Id"],
                        Codigo = (string) reader["Codigo"],
                        Nome = (string) reader["Nome"],
                        Sigla = (string) reader["Sigla"],
                        IdPais = (int) reader["IdPais"],
                        NomePais = (string) reader["NomePais"],
                        Ativo = (bool) reader["Ativo"]
                    };

                }
            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using(SqlCommand command = new SqlCommand("DELETE Estado WHERE Id= @id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                ret = (command.ExecuteNonQuery() > 0);


            }
            return ret;

        }

        public int Salvar(EstadoModel estadoModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(estadoModel.Id);

            if (model == null)
            {

                Connection();

                using (SqlCommand command = new SqlCommand("INSERT INTO Estado ( Codigo, Nome, Sigla, IdPais, Ativo ) VALUES (@Codigo, @Nome, @Sigla, @IdPais, @Ativo ); select convert(int, scope_identity())", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = estadoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = estadoModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = estadoModel.Sigla;
                    command.Parameters.AddWithValue("IdPais", SqlDbType.Int).Value = estadoModel.IdPais;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = estadoModel.Ativo;

                    ret = (int)command.ExecuteScalar();

                }

            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand("UPDATE Estado                    " +
                                                           "   SET Codigo=@codigo,           " +
                                                           "       Nome=@nome,               " +
                                                           "       Sigla=@Sigla,             " +
                                                           "       Ativo=@ativo WHERE Id=@id ", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = estadoModel.Codigo;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = estadoModel.Nome;
                    command.Parameters.AddWithValue("@Sigla", SqlDbType.VarChar).Value = estadoModel.Sigla;
                    command.Parameters.AddWithValue("@Ativo", SqlDbType.VarChar).Value = (estadoModel.Ativo ? 1 : 0);
                    command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = estadoModel.Id;

                    if (command.ExecuteNonQuery() > 0)
                    {
                        ret = estadoModel.Id;

                    }
                }
            }
            return ret;
        }
    }
}
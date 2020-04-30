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
    public class TipoPessoaRepositorio
    {
        private SqlConnection con;
        public void Connection()
        {

            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);

        }

        public List<TipoPessoaViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<TipoPessoaViewModel>();

            Connection();

            var filtroWhere = "";

            if( !string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%' ", filtro.ToLower());

            }


            var pos = (pagina - 1) * tamPag;

            var paginacao = "";

            if( pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);
            }

            using( SqlCommand command = new SqlCommand( string.Format(" SELECT *            " +
                                                                      "     FROM TipoPessoa " +
                                                                                 filtroWhere   +
                                                                      " ORDER BY Nome       " +
                                                                                 paginacao ) , con 
                                                       ) 
                )
            {
                con.Open();

                var reader = command.ExecuteReader();

                while ( reader.Read() )
                {

                    ret.Add( new TipoPessoaViewModel()
                    {
                        Id     = (int)    reader["Id"],
                        Nome   = (string) reader["Nome"],
                        Codigo = (string) reader["Codigo"],
                        Ativo  = (bool)   reader["Ativo"]
                    });
                }
                return ret;
            }
        }

        public int RecuperarQuantidade()
        {
            var ret = 0;

            Connection();

            using(SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM TipoPessoa", con))
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }
            return ret;

        }

        public TipoPessoaModel RecuperarPeloId(int id)
        {
            Connection();
            TipoPessoaModel ret = null;

            using( SqlCommand command = new SqlCommand(" SELECT * " +
                                                       "   FROM TipoPessoa " +
                                                       "  WHERE Id = @id", con ) )
            {
                con.Open();
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if( reader.Read() )
                {
                    ret = new TipoPessoaModel()
                    {
                        Id     = (int)    reader["Id"],
                        Codigo = (string) reader["Codigo"],
                        Nome   = (string) reader["Nome"],
                        Ativo  = (bool)   reader["Ativo"]
                    };
                }
            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;
            Connection();

            using( SqlCommand command = new SqlCommand(" DELETE TipoPessoa " +
                                                       "  WHERE Id = @id", con ) )
            {
                con.Open();
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }
            return ret;
        }

        public int Salvar( TipoPessoaModel tipoPessoaModel)
        {
            var ret = 0;
            var model = RecuperarPeloId(tipoPessoaModel.Id);

            if( model == null)
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" INSERT INTO TipoPessoa ( Codigo,                 " +
                                                          "                          Nome,                   " +
                                                          "                          Ativo                   " +
                                                          "                        )                         " +
                                                          "                 VALUES ( @Codigo,                " +
                                                          "                          @Nome,                  " +
                                                          "                          @Ativo                  " +
                                                          "                        );                        " +
                                                          "          select convert( int, scope_identity() ) ", con ) )
                {
                    con.Open();
                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = tipoPessoaModel.Codigo;
                    command.Parameters.AddWithValue("@Nome",   SqlDbType.VarChar).Value = tipoPessoaModel.Nome;
                    command.Parameters.AddWithValue("@Ativo",  SqlDbType.Int    ).Value = tipoPessoaModel.Ativo;


                    ret = (int)command.ExecuteScalar();
                }


            } else
            {

                Connection();

                using(SqlCommand command = new SqlCommand(" UPDATE TipoPessoa      " +
                                                          "    SET Codigo=@Codigo, " +
                                                          "        Nome=@Nome,     " +
                                                          "        Ativo=@Ativo    " +
                                                          "  WHERE Id=@Id          ", con))
                {

                    con.Open();
                    command.Parameters.AddWithValue("@Codigo", SqlDbType.VarChar).Value = tipoPessoaModel.Codigo;
                    command.Parameters.AddWithValue("@Nome",   SqlDbType.VarChar).Value = tipoPessoaModel.Nome;
                    command.Parameters.AddWithValue("@Ativo",  SqlDbType.Int    ).Value = tipoPessoaModel.Ativo;
                    command.Parameters.AddWithValue("@Id",     SqlDbType.Int    ).Value = tipoPessoaModel.Id;

                    if ( command.ExecuteNonQuery() > 0 )
                    {

                        ret = tipoPessoaModel.Id;

                    }
                }
            }
            return ret;
        }
    }
}
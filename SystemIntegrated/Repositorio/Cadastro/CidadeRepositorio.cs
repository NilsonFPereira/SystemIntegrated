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
    public class CidadeRepositorio
    {

        /**********************************************************************
         **************** CRIANDO CONEXAO COM O BANDO DE DADOS ****************
         **********************************************************************/
        private SqlConnection con;
        public void Connection()
        {
           String constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }
       
        /* ********************************************************************
         * LISTANDO TODAS AS CIDADES DE ACORDO COM OS PARÂMETRO DE FILTRAGENS *
         **********************************************************************/
        public List<CidadeViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "", int idEstado = 0)
        {
            var ret = new List<CidadeViewModel>();

            Connection();

            var pos = (pagina - 1) * tamPag;

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(CI.Nome) LIKE '%{0}%'", filtro.ToLower());


            }

            if (idEstado > 0)
            {

                filtroWhere += (string.IsNullOrEmpty(filtroWhere) ? " WHERE" : " AND")
                            + string.Format(" CI.IdEstado =  {0}", idEstado);

            }          

            var paginacao = "";
            if ( pagina > 0 && tamPag > 0 )
            {
                paginacao = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", 
                    pos, tamPag);
            }



            using ( SqlCommand command = new SqlCommand( string.Format("     SELECT CI.Id,                           " +
                                                                      "            CI.Nome,                         " +
                                                                      "            CI.Codigo,                       " +
                                                                      "            NomeEstado = ES.Nome,            " +
                                                                      "            Sigla = ES.Sigla,                " +
                                                                      "            CI.Ativo                         " +
                                                                      "       FROM Cidade CI                        " +
                                                                      " INNER JOIN Estado ES ON ES.Id = CI.IdEstado " +
                                                                                   filtroWhere                        +
                                                                      "   ORDER BY CI.Nome                          " +
                                                                                   paginacao 
                                                                      ), con 
                                                      ) 
            ) {
                con.Open();

                var reader = command.ExecuteReader();

                while ( reader.Read() ) {
                    ret.Add( new CidadeViewModel()
                    {
                        Id         = (int)    reader["Id"],
                        Nome       = (string) reader["Nome"],
                        Codigo     = (string) reader["Codigo"],
                        NomeEstado = (string) reader["NomeEstado"],
                        Sigla      = (string) reader["Sigla"],
                        Ativo      = (bool)   reader["Ativo"]
                    });
                }
                return ret;
            }
        }

        /**********************************************************************
         ********** RECUPERANDO A QUANTIDADE DE CIDADES CADASTRADAS ***********
         **********************************************************************/
        public int RecuperarQuantidade()
        {
            int ret = 0;

            Connection();

            using ( SqlCommand command = new SqlCommand(" SELECT COUNT(*) " +
                                                        "   FROM Cidade   ", con
                                                       )
                  )
            {

                con.Open();

                ret = (int) command.ExecuteScalar();
            }
            return ret;
        }

        /*********************************************************************
         ********************** RECUPERANDO CIDADE PELO ID *******************
         *********************************************************************/
        public CidadeModel RecuperarPeloId( int id )
        {

            CidadeModel ret = null;

            Connection();

            using( SqlCommand command = new SqlCommand(" SELECT *        " +
                                                       "   FROM Cidade   " +
                                                       "  WHERE Id = @id ", con 
                                                       )
            ) {

                con.Open();
                
                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;
                
                var reader = command.ExecuteReader();

                if ( reader.Read() )
                {
                    ret = new CidadeModel()
                    {
                        Id       = (int)    reader["Id"],
                        Nome     = (string) reader["Nome"],
                        Codigo   = (string) reader["Codigo"],
                        IdEstado = (int)    reader["IdEstado"],
                        Ativo    = (bool)   reader["Ativo"]
                    };
                }
            }
            return ret;
        }

        /******************************************************************** 
         ************************* INSERT E UPDATE **************************
         ********************************************************************/
        public int Salvar(CidadeModel cidadeModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(cidadeModel.Id);

            if( model == null)
            {

                Connection();

                using(SqlCommand command = new SqlCommand("INSERT INTO Cidade ( Codigo,             " +
                                                          "                     Nome,               " +
                                                          "                     IdEstado,           " +
                                                          "                     Ativo               " +
                                                          "                   )                     " +
                                                          "            VALUES ( @Codigo,            " +
                                                          "                     @Nome,              " +
                                                          "                     @IdEstado,          " +
                                                          "                     @Ativo              " +
                                                          "                   );                    " +
                                                          " select convert( int, scope_identity() ) ", con ) 
                ) {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo",   SqlDbType.VarChar).Value = cidadeModel.Codigo;
                    command.Parameters.AddWithValue("@Nome",     SqlDbType.VarChar).Value = cidadeModel.Nome;
                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value     = cidadeModel.IdEstado;
                    command.Parameters.AddWithValue("@Ativo",    SqlDbType.Int).Value     = cidadeModel.Ativo;

                    ret = (int) command.ExecuteScalar();
                }

            }else
            {
                Connection();

                using(SqlCommand command = new SqlCommand(" UPDATE Cidade              " +
                                                          "    SET Codigo=@Codigo,     " +
                                                          "        Nome=@Nome,         " +
                                                          "        IdEstado=@IdEstado, " +
                                                          "        Ativo=@Ativo        " +
                                                          "  WHERE Id=@Id              ", con))
                {
                    con.Open();

                    command.Parameters.AddWithValue("@Codigo",   SqlDbType.VarChar).Value = cidadeModel.Codigo;
                    command.Parameters.AddWithValue("@Nome",     SqlDbType.VarChar).Value = cidadeModel.Nome;
                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value     = cidadeModel.IdEstado;
                    command.Parameters.AddWithValue("@Ativo",    SqlDbType.Int).Value     = cidadeModel.Ativo;
                    command.Parameters.AddWithValue("@Id",       SqlDbType.Int).Value     = cidadeModel.Id;

                    if( command.ExecuteNonQuery() > 0)
                    {

                        ret = cidadeModel.Id;
                    }
                }
            }

            return ret;

        }

        /******************************************************************** 
         ****************************** EXLUIR ******************************
         ********************************************************************/
        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using(SqlCommand command = new SqlCommand(" DELETE Cidade" +
                                                      "  WHERE Id =@Id", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;


                ret = command.ExecuteNonQuery() > 0;

            }
            return ret;

        }
    }
}
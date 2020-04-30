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
    public class ClienteRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();

            con = new SqlConnection(constr);

        }
        public List<ClienteViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<ClienteViewModel>();

            Connection();

            var filtroWhere = "";

            if(!string.IsNullOrEmpty(filtro))
            {

                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }

            var pos = (pagina - 1) * tamPag;
            var paginacao = "";

            if(pagina > 0 && tamPag > 0)
            {

                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);


            }

            using(SqlCommand command = new SqlCommand(string.Format("     SELECT Id,                                                                    " +
										                            "            Nome,                                                                  " +
                                                                    "            DataNascimento = ISNULL(CONVERT(VARCHAR(10), DataNascimento, 103), '')," +
                                                                    "            CnpjCpf = CASE WHEN IdTipoPessoa = 1                                   " +
                                                                    "                           THEN 'CPF: '+ CnpjCpf                                   " +
                                                                    "                           WHEN IdTipoPessoa = 2                                   " +
                                                                    "                           THEN 'CNPJ: '+ CnpjCpf                                  " +
                                                                    "                           END,                                                    "+
                                                                    "            RgInscricaoEstadual = CASE WHEN IdTipoPessoa = 1                       " +
                                                                    "                                       THEN 'RG: '+ RgInscricaoEstadual            " +
                                                                    "                                       WHEN IdTipoPessoa = 2                       " +
                                                                    "                                       THEN 'I.E: '+ RgInscricaoEstadual           " +
                                                                    "                                  END,                                             " +
                                                                    "            Ativo                                                                  " +
                                                                    "       FROM Cliente                                                                " +
                                                                                 filtroWhere                                                              +
                                                                    "   ORDER BY Nome                                                                   " +
                                                                    paginacao ) , con ) )
            {
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new ClienteViewModel()
                    {
                        Id             = (int)    reader["Id"],
                        Nome           = (string) reader["Nome"],
                        DataNascimento = (string) reader["DataNascimento"],
                        CnpjCpf =    (string) reader["CnpjCpf"],
                        RgInscricaoEstadual = (string) reader["RgInscricaoEstadual"],
                        Ativo          = (bool)   reader["Ativo"]
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
                                                      "   FROM Cliente  ", con ))
            {
            con.Open();

            ret = (int)command.ExecuteScalar();

            }
            return ret;
        }
        public ClienteViewModel RecuperarPeloId(int id)
        {
            ClienteViewModel ret = null;

            Connection();
                                                                                                                                    
            using(SqlCommand command = new SqlCommand("   SELECT CL.Id,                                                                     " +
                                                      "       CL.IdTipoPessoa,                                                              " +
                                                      "       TipoPessoa = TP.Nome,                                                         " +
                                                      "       Nome = ISNULL(CL.Nome, ''),                                                   " +
                                                      "       RazaoSocial = ISNULL(CL.RazaoSocial, ''),                                     " +
                                                      "       IdSexo = ISNULL(CAST(CL.IdSexo AS VARCHAR(20)), ''),                          " +
                                                      "       Sexo = ISNULL(SE.Nome, ''),                                                               " +
                                                      "       DataNascimento = ISNULL(CONVERT(VARCHAR(10), CL.DataNascimento, 103), ''),    " +
                                                      "       CL.CnpjCpf,                                                                   " +
                                                      "       RgInscricaoEstadual = ISNULL(CL.RgInscricaoEstadual, ''),                     " +
                                                      "       CL.IdEstado,                                                                  " +
                                                      "       NomeEstado = ES.Nome,                                                         " +
                                                      "       CL.IdCidade,                                                                  " +
                                                      "       NomeCidade = CI.Nome,                                                         " +
                                                      "       CL.Logradouro,                                                                " +
                                                      "       CL.Numero,                                                                    " +
                                                      "       CL.Bairro,                                                                    " +
                                                      "       Cep = ISNULL(CL.Cep, ''),                                                     " +
                                                      "       Telefone = ISNULL(CL.Telefone, ''),                                           " +
                                                      "       Celular = ISNULL(CL.Celular, ''),                                             " +
                                                      "       Email = ISNULL(CL.Email, ''),                                                 " +
                                                      "       DataCadastro = ISNULL(CONVERT(VARCHAR(10), CL.DataCadastro, 103), ''),        " +
                                                      "       CL.Ativo                                                                      " +
                                                      "  FROM Cliente          CL                                                           " +
                                                      " INNER JOIN TipoPessoa TP ON TP.Id = CL.IdTipoPessoa                                 " +
                                                      " LEFT JOIN Sexo       SE ON SE.Id = CL.IdSexo                                       " +
                                                      " INNER JOIN Estado     ES ON ES.Id = CL.IdEstado                                          " +
                                                      " INNER JOIN Cidade     CI ON CI.Id = CL.IdCidade                                    " +
                                                      "  WHERE CL.Id=@Id ",con ) )
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    ret = new ClienteViewModel()
                    {
                        Id                  = (int)    reader["Id"],
                        IdTipoPessoa        = (int)    reader["IdTipoPessoa"],
                        TipoPessoa          = (string) reader["TipoPessoa"],
                        Nome                = (string)reader["Nome"],
                        RazaoSocial         = (string)reader["RazaoSocial"],
                        IdSexo              = (string)    reader["IdSexo"],
                        Sexo                = (string) reader["Sexo"],
                        DataNascimento      = (string)reader["DataNascimento"],
                        CnpjCpf             = (string)reader["CnpjCpf"],
                        RgInscricaoEstadual = (string)reader["RgInscricaoEstadual"],
                        IdEstado            = (int)    reader["IdEstado"],
                        NomeEstado          = (string) reader["NomeEstado"],
                        IdCidade            = (int)    reader["IdCidade"],
                        NomeCidade          = (string) reader["NomeCidade"],
                        Logradouro          = (string) reader["Logradouro"],
                        Numero              = (string) reader["Numero"],
                        Bairro              = (string) reader["Bairro"],
                        Cep                 = (string) reader["Cep"],
                        Telefone            = (string) reader["Telefone"],
                        Celular             = (string) reader["Celular"],
                        Email               = (string) reader["Email"],
                        DataCadastro        = (string) reader["DataCadastro"],
                        Ativo               = (bool)   reader["Ativo"]

                    };
                }
            }
            return ret;
        }
        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using (SqlCommand command = new SqlCommand(" DELETE Cliente " +
                                                      "  WHERE Id = @Id", con))
            {

                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;


            }
            return ret;
        }
        public int Salvar(ClienteModel clienteModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(clienteModel.Id);

            if( model == null)
            {
                Connection();

                using (SqlCommand command = new SqlCommand(" INSERT INTO Cliente ( IdTipoPessoa,        " +
                                                           "                       Nome,                " +
                                                           "                       RazaoSocial,         " +
                                                           "                       IdSexo,              " +
                                                           "                       DataNascimento,      " +
                                                           "                       CnpjCpf,             " +
                                                           "                       RgInscricaoEstadual, " +
                                                           "                       IdEstado,            " +
                                                           "                       IdCidade,            " +
                                                           "                       Logradouro,          " +
                                                           "                       Numero,              " +
                                                           "                       Bairro,              " +
                                                           "                       Cep,                 " +
                                                           "                       Telefone,            " +
                                                           "                       Celular,             " +
                                                           "                       Email,               " +
                                                           "                       DataCadastro,        " +
                                                           "                       Ativo                " +
                                                           "                    )                       " +
                                                           "             VALUES ( @IdTipoPessoa,        " +
                                                           "                      @Nome,                " +
                                                           "                      @RazaoSocial,         " +
                                                           "                      @IdSexo,              " +
                                                           "                      @DataNascimento,      " +
                                                           "                      @CnpjCpf,             " +
                                                           "                      @RgInscricaoEstadual, " +
                                                           "                      @IdEstado,            " +
                                                           "                      @IdCidade,            " +
                                                           "                      @Logradouro,          " +
                                                           "                      @Numero,              " +
                                                           "                      @Bairro,              " +
                                                           "                      @Cep,                 " +
                                                           "                      @Telefone,            " +
                                                           "                      @Celular,             " +
                                                           "                      @Email,               " +
                                                           "                      @DataCadastro,        " +
                                                           "                      @Ativo                " +
                                                           "                    );                      " +
                                                           "      select convert( int, scope_identity())", con))
                {

                    con.Open();

                    command.Parameters.AddWithValue("@IdTipoPessoa", SqlDbType.Int).Value = clienteModel.IdTipoPessoa;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = clienteModel.Nome;

                    if ( ! string.IsNullOrEmpty(clienteModel.RazaoSocial ))
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = clienteModel.RazaoSocial;
                    } else
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( clienteModel.IdSexo ) )
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.VarChar).Value = clienteModel.IdSexo;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    
                    if ( ! string.IsNullOrEmpty( clienteModel.DataNascimento ) )
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.DateTime).Value = clienteModel.DataNascimento;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.VarChar).Value = DBNull.Value;                   
                    }

                    command.Parameters.AddWithValue("@CnpjCpf", SqlDbType.VarChar).Value = clienteModel.CnpjCpf;

                    if ( !string.IsNullOrEmpty( clienteModel.RgInscricaoEstadual ) )
                    {
                        command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = clienteModel.RgInscricaoEstadual;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value = clienteModel.IdEstado;

                    command.Parameters.AddWithValue("@IdCidade", SqlDbType.Int).Value = clienteModel.IdCidade;

                    command.Parameters.AddWithValue("@Logradouro", SqlDbType.VarChar).Value = clienteModel.Logradouro;

                    command.Parameters.AddWithValue("@Numero", SqlDbType.VarChar).Value = clienteModel.Numero;

                    command.Parameters.AddWithValue("@Bairro", SqlDbType.VarChar).Value = clienteModel.Bairro;

                    if(!string.IsNullOrEmpty(clienteModel.Cep))
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = clienteModel.Cep;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = DBNull.Value;
                    }



                    if ( ! string.IsNullOrEmpty( clienteModel.Telefone ))
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = clienteModel.Telefone;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = DBNull.Value;
                    }


                    if (!string.IsNullOrEmpty(clienteModel.Celular))
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = clienteModel.Celular;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(clienteModel.Email))
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = clienteModel.Email;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@DataCadastro", SqlDbType.DateTime).Value = clienteModel.DataCadastro;

                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = clienteModel.Ativo;

                    ret = (int)command.ExecuteScalar();

                }
            }
            else
            {
                Connection();

                using (SqlCommand command = new SqlCommand(" UPDATE Cliente                                     " +
                                                           "    SET IdTipoPessoa        = @IdTipoPessoa,        " +
                                                           "        Nome                = @Nome,                " +
                                                           "        RazaoSocial         = @RazaoSocial,         " +
                                                           "        IdSexo              = @IdSexo,              " +
                                                           "        DataNascimento      = @DataNascimento,      " +
                                                           "        CnpjCpf             = @CnpjCpf,             " +
                                                           "        RgInscricaoEstadual = @RgInscricaoEstadual, " +
                                                           "        IdEstado            = @IdEstado,            " +
                                                           "        IdCidade            = @IdCidade,            " +
                                                           "        Logradouro          = @Logradouro,          " +
                                                           "        Numero              = @Numero,              " +
                                                           "        Bairro              = @Bairro,              " +
                                                           "        Cep                 = @Cep,                 " +
                                                           "        Telefone            = @Telefone,            " +
                                                           "        Celular             = @Celular,             " +
                                                           "        Email               = @Email,               " +
                                                           "        Ativo               = @Ativo                " +
                                                           "  WHERE Id                  = @Id                   ", con ) )
                {

                    con.Open();

                    command.Parameters.AddWithValue("@IdTipoPessoa", SqlDbType.Int).Value = clienteModel.IdTipoPessoa;

                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = clienteModel.Nome;

                    if (!string.IsNullOrEmpty(clienteModel.RazaoSocial))
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = clienteModel.RazaoSocial;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(clienteModel.IdSexo))
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.VarChar).Value = clienteModel.IdSexo;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(clienteModel.DataNascimento))
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.DateTime).Value = clienteModel.DataNascimento;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@CnpjCpf", SqlDbType.VarChar).Value = clienteModel.CnpjCpf;

                    if (!string.IsNullOrEmpty(clienteModel.RgInscricaoEstadual))
                    {
                        command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = clienteModel.RgInscricaoEstadual;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value = clienteModel.IdEstado;

                    command.Parameters.AddWithValue("@IdCidade", SqlDbType.Int).Value = clienteModel.IdCidade;

                    command.Parameters.AddWithValue("@Logradouro", SqlDbType.VarChar).Value = clienteModel.Logradouro;

                    command.Parameters.AddWithValue("@Numero", SqlDbType.VarChar).Value = clienteModel.Numero;

                    command.Parameters.AddWithValue("@Bairro", SqlDbType.VarChar).Value = clienteModel.Bairro;

                    if (!string.IsNullOrEmpty(clienteModel.Cep))
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = clienteModel.Cep;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = DBNull.Value;
                    }



                    if (!string.IsNullOrEmpty(clienteModel.Telefone))
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = clienteModel.Telefone;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = DBNull.Value;
                    }


                    if (!string.IsNullOrEmpty(clienteModel.Celular))
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = clienteModel.Celular;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(clienteModel.Email))
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = clienteModel.Email;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = clienteModel.Ativo;

                    command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = clienteModel.Id;


                    if( command.ExecuteNonQuery() > 0)
                    {
                        ret = clienteModel.Id;

                    }
                }
            }
            return ret;

        }

        public List<ClienteModel> ListaSuggest(string query)
        {
            var ret = new List<ClienteModel>();

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(query))
            {

                filtroWhere = string.Format(" WHERE  LOWER(Nome) LIKE '%{0}%'", query.ToLower());
            }


            Connection();

            using (SqlCommand command = new SqlCommand(" SELECT Id, Nome " +
                                                      "   FROM Cliente " +
                                                      filtroWhere +
                                                      "ORDER BY Nome", con))
            {

                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new ClienteModel()
                    {

                        Id = (int)reader["Id"],
                        Nome = (string)reader["Nome"]

                    });
                }
            }

            return ret;

        }

    }
}
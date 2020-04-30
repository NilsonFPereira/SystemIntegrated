using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIntegrated.Models;

namespace SystemIntegrated.Repositorio
{
    public class FornecedorRepositorio
    {
        private SqlConnection con;

        public void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public List<FornecedorViewModel> RecuperarLista(int pagina = 0, int tamPag = 0, string filtro = "")
        {
            var ret = new List<FornecedorViewModel>();

            Connection();
            var filtroWhere = "";

            if ( ! string.IsNullOrEmpty(filtro))
            {
                filtroWhere = string.Format(" WHERE LOWER(Nome) LIKE '%{0}%'", filtro.ToLower());

            }


            var paginacao = "";

            var pos = (pagina - 1) * tamPag;


            if ( pagina > 0 && tamPag > 0 )
            {
                paginacao = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", pos, tamPag);

            }
            using (SqlCommand command = new SqlCommand(string.Format("      SELECT Id,                                                              " +
                                                                     "             CnpjCpf = CASE WHEN IdTipoPessoa = 1                             " +
                                                                     "                            THEN 'CPF: ' + CnpjCpf                            " +
                                                                     "                            ELSE 'CNPJ: ' + CnpjCpf                           " +
                                                                     "                       END,                                                   " +
                                                                     "             RgInscricaoEstadual = CASE WHEN IdTipoPessoa = 1                 " +
                                                                     "                                        THEN 'RG: '+ RgInscricaoEstadual      " +
                                                                     "                                        ELSE 'I.E: '+ RgInscricaoEstadual     " +
                                                                     "                                    END,                                      " +
                                                                     "             Nome,                                                         " +
                                                                     "             RazaoSocial =  ISNULL( CASE WHEN IdTipoPessoa = 1                         " +
                                                                     "                                THEN ''                                       " +
                                                                     "                                ELSE RazaoSocial                              " +
                                                                     "                           END, ''),                                               " +                                                                     
                                                                     "             Ativo                                                            " +
                                                                     "        FROM Fornecedor                                                       " +
                                                                                   filtroWhere                                                        +
                                                                     "    ORDER BY Nome                                                          " +
                                                                                   paginacao ), con))
            {                                                       
                                                                    
                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new FornecedorViewModel()
                    {
                        Id = (int)reader["Id"],
                        CnpjCpf = (string)reader["CnpjCpf"],
                        RgInscricaoEstadual = (string)reader["RgInscricaoEstadual"],
                        Nome = (string)reader["Nome"],
                        RazaoSocial = (string)reader["RazaoSocial"],
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

            using(SqlCommand command = new SqlCommand(" SELECT COUNT(*)   " +
                                                      "   FROM Fornecedor ", con ))
            {
                con.Open();

                ret = (int)command.ExecuteScalar();

            }

            return ret;

        }
        public FornecedorViewModel RecuperarPeloId(int id)
        {
            FornecedorViewModel ret = null;

            Connection();

            using (SqlCommand command = new SqlCommand("     SELECT FO.Id,                                                                          " +
                                                       "            FO.IdTipoPessoa,                                                                " + 
                                                       "            Nome                = ISNULL(FO.Nome, ''),                                      " + 
                                                       "            RazaoSocial         = ISNULL(FO.RazaoSocial, ''),                               " + 
                                                       "            FO.CnpjCpf,                                                                     " + 
                                                       "            RgInscricaoEstadual = ISNULL(FO.RgInscricaoEstadual, ''), 					    " +
                                                       "            DataNascimento      = ISNULL(CONVERT(VARCHAR(10), FO.DataNascimento, 103),''),  " +
                                                       "            Telefone            = ISNULL(FO.Telefone, ''),                                  " + 
                                                       "            Celular             = ISNULL(FO.Celular,''),                                    " + 
                                                       "            Fax                 = ISNULL(FO.Fax, ''),                                       " + 
                                                       "            Email               = ISNULL(FO.Email, ''),                                     " + 
                                                       "            FO.IdEstado,                                                                    " + 
                                                       "            FO.IdCidade,																    " +
                                                       "     	    NomeCidade = CI.Nome,														    " +
                                                       "            FO.Logradouro,                                                                  " + 
                                                       "            FO.Numero,                                                                      " + 
                                                       "            FO.Bairro,                                                                      " + 
                                                       "            Cep                 = ISNULL(FO.Cep,''),                                        " + 
                                                       "            DataCadastro        = ISNULL( CONVERT(VARCHAR(10), FO.DataCadastro, 103), ''),  " + 
                                                       "            FO.Ativo,                                                                       " + 
                                                       "            IdSexo              = ISNULL(CAST(FO.IdSexo AS VARCHAR(20)),'') ,               " + 
                                                       "     	    Site                 = ISNULL(FO.Site, '')									    " +
                                                       "       FROM Fornecedor  FO																    " +
                                                       " INNER JOIN Cidade CI ON CI.Id = FO.IdCidade											    " +                                                      
                                                       "      WHERE FO.Id=@Id                                                                       ", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                var reader = command.ExecuteReader();

                if (reader.Read())
                {

                    ret = new FornecedorViewModel()
                    {
                        Id = (int)reader["Id"],
                        IdTipoPessoa = (int) reader["IdTipoPessoa"],
                        CnpjCpf = (string)reader["CnpjCpf"],
                        RgInscricaoEstadual = (string)reader["RgInscricaoEstadual"],
                        Nome = (string)reader["Nome"],
                        RazaoSocial = (string) reader["RazaoSocial"],
                        DataNascimento = (string) reader["DataNascimento"],
                        Telefone = (string)reader["Telefone"],
                        Celular = (string)reader["Celular"],
                        Fax = (string)reader["Fax"],
                        Email = (string)reader["Email"],
                        IdEstado = (int)reader["IdEstado"],
                        IdCidade = (int)reader["IdCidade"],
                        NomeCidade =(string) reader["NomeCidade"],
                        Logradouro = (string)reader["Logradouro"],
                        Numero = (string)reader["Numero"],
                        Bairro = (string)reader["Bairro"],
                        Cep = (string)reader["Cep"],
                        DataCadastro = (string)reader["DataCadastro"],
                        Ativo = (bool)reader["Ativo"],
                        IdSexo = (string)reader["IdSexo"],
                        Site = (string) reader["Site"]

                    };
                }
            }
            return ret;
        }

        public bool ExcluirPeloId(int id)
        {
            var ret = false;

            Connection();

            using (SqlCommand command = new SqlCommand(" DELETE Fornecedor" +
                                                      "  WHERE Id=@Id", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                ret = (int)command.ExecuteNonQuery() > 0;

            }

            return ret;
        }

        public int Salvar(FornecedorModel fornecedorModel)
        {
            var ret = 0;

            var model = RecuperarPeloId(fornecedorModel.Id);

            if (model == null)
            {
                Connection();


                using (SqlCommand command = new SqlCommand("INSERT INTO Fornecedor ( IdTipoPessoa,         " +
                                                          "                          Nome,                 " +
                                                          "                          RazaoSocial,          " +
                                                          "                          IdSexo,               " +
                                                          "                          DataNascimento,       " +
                                                          "                          CnpjCpf,              " +
                                                          "                          RgInscricaoEstadual,  " +
                                                          "                          IdEstado,             " +
                                                          "                          IdCidade,             " +
                                                          "                          Logradouro,           " +
                                                          "                          Numero,               " +
                                                          "                          Bairro,               " +
                                                          "                          Cep,                  " +
                                                          "                          Telefone,             " +
                                                          "                          Celular,              " +
                                                          "                          Fax,                  " +
                                                          "                          Email,                " +
                                                          "                          DataCadastro,         " +
                                                          "                          Site,                 " +
                                                          "                          Ativo                 " +
                                                          "                        )                       " +
                                                          "                 VALUES ( @IdTipoPessoa,        " +
                                                          "                          @Nome,                " +
                                                          "                          @RazaoSocial,         " +
                                                          "                          @IdSexo,              " +
                                                          "                          @DataNascimento,      " +
                                                          "                          @CnpjCpf,             " +
                                                          "                          @RgInscricaoEstadual, " +
                                                          "                          @IdEstado,            " +
                                                          "                          @IdCidade,            " +
                                                          "                          @Logradouro,          " +
                                                          "                          @Numero,              " +
                                                          "                          @Bairro,              " +
                                                          "                          @Cep,                 " +
                                                          "                          @Telefone,            " +
                                                          "                          @Celular,             " +
                                                          "                          @Fax,                 " +
                                                          "                          @Email,               " +
                                                          "                          @DataCadastro,        " +
                                                          "                          @Site,                " +
                                                          "                          @Ativo                " +
                                                          "                        );                      " +
                                                          "          select convert( int, scope_identity())", con))
                {                                                  

                    con.Open();

                    command.Parameters.AddWithValue("@IdTipoPessoa", SqlDbType.Int).Value = fornecedorModel.IdTipoPessoa;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = fornecedorModel.Nome;

                    if( !string.IsNullOrEmpty(fornecedorModel.RazaoSocial)) {                         
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = fornecedorModel.RazaoSocial;                    
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.IdSexo ) )
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.Int).Value = fornecedorModel.IdSexo;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.Int).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.DataNascimento ) )
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.DateTime).Value = fornecedorModel.DataNascimento;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@CnpjCpf", SqlDbType.VarChar).Value = fornecedorModel.CnpjCpf;
                    command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = fornecedorModel.RgInscricaoEstadual;
                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value = fornecedorModel.IdEstado;
                    command.Parameters.AddWithValue("@IdCidade", SqlDbType.Int).Value = fornecedorModel.IdCidade;
                    command.Parameters.AddWithValue("@Logradouro", SqlDbType.VarChar).Value = fornecedorModel.Logradouro;
                    command.Parameters.AddWithValue("@Numero", SqlDbType.VarChar).Value = fornecedorModel.Numero;
                    command.Parameters.AddWithValue("@Bairro", SqlDbType.VarChar).Value = fornecedorModel.Bairro;

                    if ( ! string.IsNullOrEmpty( fornecedorModel.Cep ) )
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = fornecedorModel.Cep;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.Telefone ) )
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = fornecedorModel.Telefone;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.Celular ) )
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = fornecedorModel.Celular;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.Fax ) )
                    {
                        command.Parameters.AddWithValue("@Fax", SqlDbType.VarChar).Value = fornecedorModel.Fax;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Fax", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if ( ! string.IsNullOrEmpty( fornecedorModel.Email ) )
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = fornecedorModel.Email;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Site))
                    {
                        command.Parameters.AddWithValue("@Site", SqlDbType.VarChar).Value = fornecedorModel.Site;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Site", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@DataCadastro", SqlDbType.DateTime).Value = fornecedorModel.DataCadastro;

                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = fornecedorModel.Ativo;

                    ret = (int)command.ExecuteScalar();
                }

            }
            else
            {

                Connection();


                using (SqlCommand command = new SqlCommand(" UPDATE Fornecedor                                   " +
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
                                                            "        Fax                 = @Fax,                 " +
                                                            "        Email               = @Email,               " +
                                                            "        Site                = @Site,                " +
                                                            "        Ativo               = @Ativo                " +
                                                            "  WHERE Id                  = @Id                   ", con ) )
                {

                    con.Open();

                    command.Parameters.AddWithValue("@IdTipoPessoa", SqlDbType.Int).Value = fornecedorModel.IdTipoPessoa;
                    command.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = fornecedorModel.Nome;

                    if (!string.IsNullOrEmpty(fornecedorModel.RazaoSocial))
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = fornecedorModel.RazaoSocial;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.IdSexo))
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.Int).Value = fornecedorModel.IdSexo;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@IdSexo", SqlDbType.Int).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.DataNascimento))
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.DateTime).Value = fornecedorModel.DataNascimento;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DataNascimento", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@CnpjCpf", SqlDbType.VarChar).Value = fornecedorModel.CnpjCpf;
                    command.Parameters.AddWithValue("@RgInscricaoEstadual", SqlDbType.VarChar).Value = fornecedorModel.RgInscricaoEstadual;
                    command.Parameters.AddWithValue("@IdEstado", SqlDbType.Int).Value = fornecedorModel.IdEstado;
                    command.Parameters.AddWithValue("@IdCidade", SqlDbType.Int).Value = fornecedorModel.IdCidade;
                    command.Parameters.AddWithValue("@Logradouro", SqlDbType.VarChar).Value = fornecedorModel.Logradouro;
                    command.Parameters.AddWithValue("@Numero", SqlDbType.VarChar).Value = fornecedorModel.Numero;
                    command.Parameters.AddWithValue("@Bairro", SqlDbType.VarChar).Value = fornecedorModel.Bairro;

                    if (!string.IsNullOrEmpty(fornecedorModel.Cep))
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = fornecedorModel.Cep;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Cep", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Telefone))
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = fornecedorModel.Telefone;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Telefone", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Celular))
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = fornecedorModel.Celular;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Celular", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Fax))
                    {
                        command.Parameters.AddWithValue("@Fax", SqlDbType.VarChar).Value = fornecedorModel.Fax;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Fax", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Email))
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = fornecedorModel.Email;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    if (!string.IsNullOrEmpty(fornecedorModel.Site))
                    {
                        command.Parameters.AddWithValue("@Site", SqlDbType.VarChar).Value = fornecedorModel.Site;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Site", SqlDbType.VarChar).Value = DBNull.Value;
                    }

                    command.Parameters.AddWithValue("@Ativo", SqlDbType.Int).Value = fornecedorModel.Ativo; command.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = fornecedorModel.Id;



                    if (command.ExecuteNonQuery() > 0)
                    {
                        ret = fornecedorModel.Id;

                    }
                }                
            }
            return ret;
        }

        public List<FornecedorModel> ListaSuggest(string query)
        {
            var ret = new List<FornecedorModel>();

            var filtroWhere = "";

            if (!string.IsNullOrEmpty(query))
            {

                filtroWhere = string.Format(" WHERE  LOWER(Nome) LIKE '%{0}%'", query.ToLower());
            }


            Connection();

            using (SqlCommand command = new SqlCommand(" SELECT Id, Nome " +
                                                      "   FROM Fornecedor " +
                                                      filtroWhere +
                                                      "ORDER BY Nome", con))
            {

                con.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ret.Add(new FornecedorModel()
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
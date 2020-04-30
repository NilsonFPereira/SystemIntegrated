using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SystemIntegrated.Models
{
    public class NivelUsuarioModel
    {
        private SqlConnection con;

        public void Connection()
        {
            String constr = ConfigurationManager.ConnectionStrings["stringConexao"].ToString();
            con = new SqlConnection(constr);
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage="Informe o código.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public List<UsuarioModel> Usuarios { get; set; }

        public NivelUsuarioModel()
        {
            this.Usuarios = new List<UsuarioModel>();
        }

        public void CarregarUsuarios()
        {
            this.Usuarios.Clear();

            Connection();

            using(SqlCommand command = new SqlCommand(" SELECT US.* " +
                                                      "  FROM Usuario US, Nivel_Usuario NU  " +
                                                      " WHERE ( NU.IdNivelUsuario = @id) AND (NU.IdUsuario = US.Id)", con))
            {
                con.Open();

                command.Parameters.AddWithValue("@id", SqlDbType.Int).Value = this.Id;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    this.Usuarios.Add(new UsuarioModel()
                    {
                        Id = (int)reader["Id"],
                        Nome = (string) reader["Nome"],
                        Usuario = (string) reader["Usuario"],
                        Cpf = (string)reader["Cpf"],
                        Email = (string) reader["Email"]

                    });

                }

            }
        }
    }
}
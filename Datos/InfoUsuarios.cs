using EjApi.Modelo;
using MySql.Data.MySqlClient;
using Dapper;

namespace EjApi.Datos
{
    public class InfoUsuarios
    {
        private string connectionString = @"Server=localhost;Port=3306;Database=crudaspext;Uid=root;Pwd=ivandg01";

        public async Task<Usuario> CrearUsuario(string nombre, string mail, string pass)
        {
            var user = new Usuario();
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    var param = new { Nombre = nombre, Mail = mail, Pass = pass};
                    var sql = @"INSERT INTO usuarios (Nombre_Usuario, Mail_Usuario, Pass_Usuario)
                           VALUES (@Nombre, @Mail, @Pass);";
                    
                    var results = await connection.ExecuteAsync(sql,param);
                    Console.WriteLine($"Registro insertado a BD Correctamente :)");
                    return new Usuario { Nombre_Usuario = nombre, Mail_Usuario = mail };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error, no se inserto correctamente el nuevo usuario :( {e.Message}");
                return user;
            }
        }

        public async Task<Usuario> ObtenerUsuarioID(int id)
        {
                var user = new Usuario();
            
                using (var connection = new MySqlConnection(connectionString))
                {
                    var parameters = new { Id = id };
                    var sql = @"SELECT Id_Usuario, Nombre_Usuario, Mail_Usuario 
                                FROM usuarios
                                WHERE Id_Usuario=@id";
                    user = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, parameters);
                }


            if (user == null)
            {
                Console.WriteLine("Error, no se encontro el Usuario :(");
                return null;
            }
            else
            {
                Console.WriteLine("Usuario encontrado :)");
                return user;
            }
        }

        public async Task<Usuario> ModificarUsuario(int id, string nombre,string mail,string pass)
        {
            var user = new Usuario();
            try
            {

                using (var connection = new MySqlConnection(connectionString))
                {
                    var param = new { Id = id, Nombre = nombre, Mail = mail, Pass = pass};
                    var sql = @"UPDATE usuarios 
                                SET Nombre_Usuario = @Nombre,
                                    Mail_Usuario = @Mail,
                                    Pass_Usuario = @Pass
                                WHERE Id_Usuario = @Id";
                    var results = await connection.ExecuteAsync(sql, param);
                    //Console.WriteLine($"Registro modificado en BD Correctamente :)");
                    return new Usuario { Nombre_Usuario = nombre};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error, no se logro modificar el usuario :( {e.Message}");
                return user;
            }
        }

        public async Task<Usuario> EliminarUsuario(int id)
        {
            var user = new Usuario();
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    var param = new { Id = id };
                    var sql = @"DELETE FROM usuarios
                                WHERE Id_Usuario=@Id";

                    var results = await connection.ExecuteAsync(sql, param);
                    //Console.WriteLine($"Registro eliminado de BD Correctamente :)");
                    return new Usuario {};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error, no se logro eliminar correctamente el usuario :( {e.Message}");
                return user;
            }
        }

    }
}

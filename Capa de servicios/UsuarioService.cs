using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Capa_de_servicios
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> RegistrarUsuarioAsync(string nombre, string contrasenaHash, string rol)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.usp_RegistrarUsuario", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Contrasena", contrasenaHash);
            cmd.Parameters.AddWithValue("@Rol", rol);

            var pOut = new SqlParameter("@UsuarioID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pOut);

            await cmd.ExecuteNonQueryAsync();

            return pOut.Value != DBNull.Value ? (int)pOut.Value : 0;
        }
    }
}

// Program.cs (MigrationTool) - reemplaza el contenido actual
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

class Program
{
    // AJUSTA si hace falta
    static string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

    static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    static byte[] HashPassword(string password, byte[] salt, int iterations = 100_000, int length = 32)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password ?? "", salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(length);
    }

    static void Main()
    {
        Console.WriteLine("=== MigrationTool: inicio ===");
        Console.WriteLine($"ConnectionString: {connectionString}");
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            // Mostrar a qué BD estamos conectados (verifica que sea la PaintsDB esperada)
            using (var curCmd = new SqlCommand("SELECT DB_NAME()", conn))
            {
                Console.WriteLine("Conectado a base de datos: " + curCmd.ExecuteScalar());
            }

            // Contar usuarios y filas a migrar
            using (var countCmd = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.Usuario", conn))
            {
                Console.WriteLine("Total usuarios en tabla Usuario: " + countCmd.ExecuteScalar());
            }

            using (var needCmd = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.Usuario WHERE Contrasena IS NOT NULL AND (PasswordHash IS NULL OR PasswordSalt IS NULL)", conn))
            {
                int need = Convert.ToInt32(needCmd.ExecuteScalar());
                Console.WriteLine("Usuarios a migrar (Contrasena no nula y sin hash): " + need);
            }

            // Leer usuarios a migrar
            var usuarios = new List<(int id, string? pass)>();
            using (var selectCmd = new SqlCommand(
                "SELECT UsuarioID, Contrasena FROM dbo.Usuario WHERE Contrasena IS NOT NULL AND (PasswordHash IS NULL OR PasswordSalt IS NULL)", conn))
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string? pass = reader.IsDBNull(1) ? null : reader.GetString(1);
                    usuarios.Add((id, pass));
                }
            }

            Console.WriteLine($"Usuarios leídos para migrar: {usuarios.Count}");

            // Migrar uno por uno
            foreach (var u in usuarios)
            {
                Console.WriteLine($"Migrando UsuarioID={u.id} (pass {(u.pass == null ? "<NULL>" : "<hidden>")}) ...");
                try
                {
                    byte[] salt = GenerateSalt(16);
                    byte[] hash = HashPassword(u.pass ?? "", salt);

                    using var updateCmd = new SqlCommand(
                        "UPDATE dbo.Usuario SET PasswordHash = @hash, PasswordSalt = @salt WHERE UsuarioID = @id", conn);
                    updateCmd.Parameters.Add("@hash", SqlDbType.VarBinary, hash.Length).Value = hash;
                    updateCmd.Parameters.Add("@salt", SqlDbType.VarBinary, salt.Length).Value = salt;
                    updateCmd.Parameters.Add("@id", SqlDbType.Int).Value = u.id;

                    int affected = updateCmd.ExecuteNonQuery();
                    Console.WriteLine($"  -> Affected rows: {affected}");
                }
                catch (Exception exU)
                {
                    Console.WriteLine($"  !!! Error migrando usuario {u.id}: {exU.Message}");
                }
            }

            Console.WriteLine("Migración completada. Ahora verificando en BD...");

            // Verificación: muestra cuantos ya tienen hash
            using (var verifyCmd = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.Usuario WHERE PasswordHash IS NOT NULL AND PasswordSalt IS NOT NULL", conn))
            {
                Console.WriteLine("Usuarios con PasswordHash/PasswordSalt no nulos: " + verifyCmd.ExecuteScalar());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR GENERAL: " + ex.ToString());
        }
        Console.WriteLine("=== MigrationTool: fin ===");
        Console.WriteLine("Pulsa una tecla para salir...");
        Console.ReadKey();
    }
}

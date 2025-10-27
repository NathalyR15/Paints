using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Paints
{
    public partial class Inicio : Form
    {
        private string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text.Trim();
            string pass = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Ingresa usuario y contraseña.");
                return;
            }

            string sql = "SELECT UsuarioID, Rol, PasswordHash, PasswordSalt FROM Usuario WHERE Nombre = @Nombre";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 255).Value = usuario;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Usuario no encontrado.");
                            return;
                        }

                        int usuarioId = Convert.ToInt32(reader["UsuarioID"]);
                        string rol = reader["Rol"]?.ToString() ?? string.Empty;

                        if (reader.IsDBNull(reader.GetOrdinal("PasswordHash")) || reader.IsDBNull(reader.GetOrdinal("PasswordSalt")))
                        {
                            MessageBox.Show("La cuenta no tiene una contraseña segura configurada.");
                            return;
                        }

                        byte[] storedHash = (byte[])reader["PasswordHash"];
                        byte[] storedSalt = (byte[])reader["PasswordSalt"];

                        byte[] computed;
                        using (var pbkdf2 = new Rfc2898DeriveBytes(pass, storedSalt, 100_000, HashAlgorithmName.SHA256))
                        {
                            computed = pbkdf2.GetBytes(storedHash.Length);
                        }

                        if (!CryptographicOperations.FixedTimeEquals(computed, storedHash))
                        {
                            MessageBox.Show("Contraseña incorrecta.");
                            // opcional: registrar intento fallido en LoginLog con estado 'FAILED'
                            try
                            {
                                InsertLoginLog(usuarioId, "FAILED", "Intento con contraseña incorrecta");
                            }
                            catch { /* no bloquear login por fallo en logging */ }
                            return;
                        }

                        // ✅ Login correcto: insertar registro en LoginLog y obtener LoginID
                        int loginId = 0;
                        try
                        {
                            loginId = InsertLoginLog(usuarioId, "OK", "Login exitoso desde WinForms");
                        }
                        catch (Exception exLog)
                        {
                            // no impedir el login por fallo en logging, pero avisar en debug
                            MessageBox.Show("Warning: no se pudo registrar LoginLog: " + exLog.Message);
                        }

                        // Abrir el formulario según rol y al cerrar actualizar el logout usando loginId
                        if (rol.Equals("Digitador", StringComparison.OrdinalIgnoreCase))
                        {
                            var fGestion = new ClientesForm(usuarioId, rol);
                            fGestion.FormClosed += (s, args) =>
                            {
                                // actualizar logout
                                if (loginId > 0) UpdateLogoutLog(loginId, "LOGOUT");
                                this.Show();
                            };
                            fGestion.Show();
                            this.Hide();
                        }
                        else if (rol.Equals("Cajero", StringComparison.OrdinalIgnoreCase))
                        {
                            var fPagos = new FacturacionForm();
                            fPagos.FormClosed += (s, args) =>
                            {
                                if (loginId > 0) UpdateLogoutLog(loginId, "LOGOUT");
                                this.Show();
                            };
                            fPagos.Show();
                            this.Hide();
                        }
                        else if (rol.Equals("Gerente", StringComparison.OrdinalIgnoreCase))
                        {
                            // Abrir UsuariosForm y pasar usuarioId, rol y loginId
                            var fUsuarios = new UsuariosForm();
                            fUsuarios.FormClosed += (s, args) =>
                            {
                                if (loginId > 0) UpdateLogoutLog(loginId, "LOGOUT");
                                this.Show();
                            };
                            fUsuarios.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Rol no reconocido.");
                            if (loginId > 0) UpdateLogoutLog(loginId, "UNKNOWN_ROLE");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frmRegistro = new Registro();
            frmRegistro.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
        }

        // ------------------ Helper methods for LoginLog ------------------

        /// <summary>
        /// Inserta un registro en LoginLog y devuelve el LoginID (SCOPE_IDENTITY).
        /// </summary>
        private int InsertLoginLog(int usuarioId, string estado = "OK", string? info = null)
        {
            const string sql = @"
                INSERT INTO dbo.LoginLog (UsuarioID, FechaLogin, Estado, Info)
                VALUES (@UsuarioID, SYSUTCDATETIME(), @Estado, @Info);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@UsuarioID", System.Data.SqlDbType.Int).Value = usuarioId;
            cmd.Parameters.Add("@Estado", System.Data.SqlDbType.NVarChar, 50).Value = estado ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Info", System.Data.SqlDbType.NVarChar, 200).Value = info is null ? DBNull.Value : info;


            conn.Open();
            object res = cmd.ExecuteScalar();
            return Convert.ToInt32(res);
        }

        /// <summary>
        /// Actualiza FechaLogout y Estado del LoginLog para el loginId dado.
        /// </summary>
        private void UpdateLogoutLog(int loginId, string estado = "LOGOUT")
        {
            const string sql = @"
                UPDATE dbo.LoginLog
                SET FechaLogout = SYSUTCDATETIME(), Estado = @Estado
                WHERE LoginID = @LoginID;";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@LoginID", System.Data.SqlDbType.Int).Value = loginId;
            cmd.Parameters.Add("@Estado", System.Data.SqlDbType.NVarChar, 50).Value = estado ?? (object)DBNull.Value;

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

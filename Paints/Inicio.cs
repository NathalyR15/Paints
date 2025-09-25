using System;
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

            // Traemos UsuarioID y Rol
            string sql = "SELECT UsuarioID, Rol FROM Usuario WHERE Nombre = @Nombre AND Contrasena = @Contrasena";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 255).Value = usuario;
                    cmd.Parameters.Add("@Contrasena", System.Data.SqlDbType.NVarChar, 255).Value = pass;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.");
                            return;
                        }

                        int usuarioId = Convert.ToInt32(reader["UsuarioID"]);
                        string rol = reader["Rol"]?.ToString() ?? string.Empty;

                        if (rol.Equals("Digitador", StringComparison.OrdinalIgnoreCase))
                        {
                            var fGestion = new ClientesForm(usuarioId, rol); // <-- ahora pasamos el int correcto
                            fGestion.FormClosed += (s, args) => this.Show();
                            MessageBox.Show("Bienvenido Digitador");
                            fGestion.Show();
                            this.Hide();
                        }
                        else if (rol.Equals("Cajero", StringComparison.OrdinalIgnoreCase))
                        {
                            var fPagos = new FormaPago();
                            fPagos.FormClosed += (s, args) => this.Show();
                            MessageBox.Show("Bienvenido Cajero");
                            fPagos.Show();
                            this.Hide();
                        }
                        else if (rol.Equals("Gerente", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Bienvenido Gerente");
                        }
                        else
                        {
                            MessageBox.Show("Rol no reconocido.");
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
            Registro frmRegistro = new Registro();
            frmRegistro.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}

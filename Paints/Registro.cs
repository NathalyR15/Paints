using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // ✅ Usar el nuevo paquete

namespace Paints
{
    public partial class Registro : Form
    {
        private string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

        public Registro()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Por favor ingresa el nombre de usuario.");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Por favor ingresa la contraseña.");
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un rol.");
                return;
            }
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden");
                return;
            }

            string nombre = textBox1.Text.Trim();
            string contrasena = textBox2.Text;
            string rol = comboBox1.SelectedItem.ToString();

            string sql = @"
                INSERT INTO Usuario (Nombre, Contrasena, Rol)
                VALUES (@Nombre, @Contrasena, @Rol);
                SELECT SCOPE_IDENTITY();
            ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasena);
                    cmd.Parameters.AddWithValue("@Rol", rol);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    int nuevoId = Convert.ToInt32(result);

                    MessageBox.Show($"Usuario registrado con éxito. ID generado = {nuevoId}");
                    LimpiarFormulario();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error en la base de datos: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            textBox1.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
            textBox3.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inicio frmInicio = new Inicio();

            // Mostrar el formulario de inicio
            frmInicio.Show();

            // Cerrar el formulario actual (Registro)
            this.Hide();
        }
    }
}

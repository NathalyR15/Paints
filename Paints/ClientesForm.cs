using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Paints
{
    public partial class ClientesForm : Form
    {
        private readonly int _usuarioId;
        private readonly string _rol;
        private int _selectedClienteId = 0;

        
        private string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

        public ClientesForm(int usuario, string rol)
        {
            InitializeComponent();
            _usuarioId = usuario;
            _rol = rol;

            // Si el diseñador no lo hizo, enganchamos eventos aquí:
            this.Load += ClientesForm_Load;
            button1.Click += button1_Click;         // Nuevo
            button2.Click += button2_Click;         // Guardar
            button3.Click += button3_Click;         // Eliminar
            dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;
            dgvClientes.KeyDown += dgvClientes_KeyDown; // Supr para eliminar
        }

        private object OrDbNull(string txt) =>
            string.IsNullOrWhiteSpace(txt) ? (object)DBNull.Value : txt.Trim();

        private void ClientesForm_Load(object sender, EventArgs e)
        {
            lblStatus.Visible = false;

            // Configuración del grid
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.AutoGenerateColumns = true;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;

            try
            {
                LoadClientes();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inicializando el formulario: " + ex.Message);
            }
        }

        private void LoadClientes(string filtro = null)
        {
            try
            {
                string sql = @"SELECT ClienteID, Nombre, Apellido, Telefono, Correo FROM dbo.Cliente";
                if (!string.IsNullOrWhiteSpace(filtro))
                    sql += " WHERE Nombre LIKE @f OR Apellido LIKE @f";
                sql += " ORDER BY Nombre";

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filtro))
                        cmd.Parameters.Add("@f", System.Data.SqlDbType.NVarChar, 255).Value = "%" + filtro.Trim() + "%";

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        dgvClientes.DataSource = dt;

                        if (dgvClientes.Columns.Contains("ClienteID"))
                            dgvClientes.Columns["ClienteID"].Visible = false;

                        lblStatus.Text = $"Registros: {dt.Rows.Count}";
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error cargando clientes (SQL): " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando clientes: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            _selectedClienteId = 0;
            textBox1.Clear(); // Nombre
            textBox2.Clear(); // Apellido
            textBox3.Clear(); // Telefono
            textBox4.Clear(); // Correo
            textBox1.Focus();
            lblStatus.Visible = true;
            lblStatus.Text = "Listo para ingresar nuevo cliente.";
            dgvClientes.ClearSelection();
        }

        // NUEVO
        private void button1_Click(object sender, EventArgs e) => ClearFields();

        // GUARDAR (Insert/Update)
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                textBox1.Focus();
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (_selectedClienteId == 0)
                    {
                        const string insertSql = @"
                            INSERT INTO dbo.Cliente (Nombre, Apellido, Telefono, Correo)
                            VALUES (@Nombre, @Apellido, @Telefono, @Correo);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (var cmd = new SqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 255).Value = textBox1.Text.Trim();
                            cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 255).Value = OrDbNull(textBox2.Text);
                            cmd.Parameters.Add("@Telefono", System.Data.SqlDbType.NVarChar, 50).Value = OrDbNull(textBox3.Text);
                            cmd.Parameters.Add("@Correo", System.Data.SqlDbType.NVarChar, 255).Value = OrDbNull(textBox4.Text);

                            int newId = (int)cmd.ExecuteScalar();
                            MessageBox.Show($"Cliente creado correctamente. ID = {newId}");
                        }
                    }
                    else
                    {
                        const string updateSql = @"
                            UPDATE dbo.Cliente
                               SET Nombre  = @Nombre,
                                   Apellido = @Apellido,
                                   Telefono = @Telefono,
                                   Correo   = @Correo
                             WHERE ClienteID = @ClienteID;";

                        using (var cmd = new SqlCommand(updateSql, conn))
                        {
                            cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar, 255).Value = textBox1.Text.Trim();
                            cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.NVarChar, 255).Value = OrDbNull(textBox2.Text);
                            cmd.Parameters.Add("@Telefono", System.Data.SqlDbType.NVarChar, 50).Value = OrDbNull(textBox3.Text);
                            cmd.Parameters.Add("@Correo", System.Data.SqlDbType.NVarChar, 255).Value = OrDbNull(textBox4.Text);
                            cmd.Parameters.Add("@ClienteID", System.Data.SqlDbType.Int).Value = _selectedClienteId;

                            int affected = cmd.ExecuteNonQuery();
                            MessageBox.Show($"Cliente actualizado ({affected} registro(s) afectado(s)).");
                        }
                    }
                }

                LoadClientes();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al guardar (SQL): " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        // ELIMINAR
        private void button3_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedClienteId(out int clienteId))
            {
                MessageBox.Show("Seleccione un cliente para eliminar.");
                return;
            }

            if (MessageBox.Show("¿Eliminar cliente seleccionado?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM dbo.Cliente WHERE ClienteID = @ClienteID", conn))
                {
                    cmd.Parameters.Add("@ClienteID", System.Data.SqlDbType.Int).Value = clienteId;
                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                        MessageBox.Show("Cliente eliminado.");
                    else
                        MessageBox.Show("No se encontró el registro a eliminar.");
                }

                LoadClientes();
                ClearFields();
            }
            catch (SqlException ex) when (ex.Number == 547) // FK constraint
            {
                MessageBox.Show("No se puede eliminar: el cliente está referenciado por otras tablas.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        // Obtiene el ID de la fila seleccionada (aunque la columna esté oculta)
        private bool TryGetSelectedClienteId(out int clienteId)
        {
            clienteId = 0;

            if (dgvClientes.CurrentRow == null || dgvClientes.CurrentRow.DataBoundItem == null)
                return false;

            if (dgvClientes.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("ClienteID") && drv["ClienteID"] != DBNull.Value)
                {
                    clienteId = Convert.ToInt32(drv["ClienteID"]);
                    _selectedClienteId = clienteId; // sincroniza el campo
                    return true;
                }
            }
            else if (dgvClientes.Columns.Contains("ClienteID"))
            {
                var cell = dgvClientes.CurrentRow.Cells["ClienteID"].Value;
                if (cell != null && cell != DBNull.Value)
                {
                    clienteId = Convert.ToInt32(cell);
                    _selectedClienteId = clienteId;
                    return true;
                }
            }

            return false;
        }

        // Sincroniza controles al cambiar de fila
        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (!TryGetSelectedClienteId(out int id))
                return;

            if (dgvClientes.CurrentRow?.DataBoundItem is DataRowView drv)
            {
                textBox1.Text = drv["Nombre"]?.ToString();
                textBox2.Text = drv["Apellido"]?.ToString();
                textBox3.Text = drv["Telefono"]?.ToString();
                textBox4.Text = drv["Correo"]?.ToString();
                lblStatus.Text = $"Editando ClienteID={id}";
            }
        }

        // Borrar con tecla Supr desde el grid
        private void dgvClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                button3_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
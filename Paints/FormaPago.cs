using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Paints
{
    public partial class FormaPago : Form
    {
        private int _usuarioId = 0;
        private string _rol = string.Empty;
        private int _selectedPagoId = 0;

        // ⚠️ Ajusta servidor/BD si aplica
        private string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

        // Requerido por el diseñador
        public FormaPago()
        {
            InitializeComponent();

            // Si el diseñador no dejó estos eventos enganchados, lo hacemos aquí:
            this.Load += FormaPago_Load;
            button1.Click += button1_Click; // Nuevo
            button2.Click += button2_Click; // Guardar
            button3.Click += button3_Click; // Eliminar
            dgvFormas.SelectionChanged += dgvFormas_SelectionChanged;
            dgvFormas.KeyDown += dgvFormas_KeyDown; // Supr para eliminar
        }

        // Constructor opcional con datos de sesión
        public FormaPago(int usuarioId, string rol) : this()
        {
            _usuarioId = usuarioId;
            _rol = rol ?? string.Empty;
        }

        private void FormaPago_Load(object sender, EventArgs e)
        {
            // Combo tipos de pago
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "Efectivo", "Cheque", "Tarjeta" });
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            // DataGridView
            dgvFormas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFormas.MultiSelect = false;
            dgvFormas.AutoGenerateColumns = true;
            dgvFormas.ReadOnly = true;
            dgvFormas.AllowUserToAddRows = false;

            LoadFormas();
            ClearFields();
        }

        private void LoadFormas()
        {
            try
            {
                const string sql = @"SELECT PagoID, TipoPago, Monto
                                     FROM dbo.FormaPago
                                     ORDER BY PagoID DESC";

                using var conn = new SqlConnection(connectionString);
                using var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                dgvFormas.DataSource = dt;

                // Ocultar la PK pero mantenerla en el DataSource
                if (dgvFormas.Columns.Contains("PagoID"))
                    dgvFormas.Columns["PagoID"].Visible = false;

                // Formato del monto
                if (dgvFormas.Columns.Contains("Monto"))
                    dgvFormas.Columns["Monto"].DefaultCellStyle.Format = "N2";

                lblStatus.Text = $"Registros: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando formas de pago: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            _selectedPagoId = 0;
            textBox1.Clear(); // monto
            if (comboBox1.Items.Count > 0 && comboBox1.SelectedIndex < 0) comboBox1.SelectedIndex = 0;
            textBox1.Focus();
            lblStatus.Text = "Listo para nueva forma de pago.";
            dgvFormas.ClearSelection();
        }

        // Botón Nuevo
        private void button1_Click(object sender, EventArgs e) => ClearFields();

        // Botón Guardar (Insert / Update)
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un tipo de pago.");
                comboBox1.Focus();
                return;
            }

            // Acepta coma o punto según cultura
            if (!decimal.TryParse(textBox1.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal monto))
            {
                // Intento alterno con cultura invariante
                if (!decimal.TryParse(textBox1.Text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out monto))
                {
                    MessageBox.Show("Ingresa un monto válido (ej: 1500.00).");
                    textBox1.Focus();
                    return;
                }
            }

            if (monto < 0)
            {
                MessageBox.Show("El monto no puede ser negativo.");
                textBox1.Focus();
                return;
            }

            string tipoPago = comboBox1.SelectedItem.ToString();

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                if (_selectedPagoId == 0)
                {
                    const string insertSql = @"
                        INSERT INTO dbo.FormaPago (TipoPago, Monto)
                        VALUES (@TipoPago, @Monto);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using var cmd = new SqlCommand(insertSql, conn);
                    cmd.Parameters.Add("@TipoPago", SqlDbType.NVarChar, 100).Value = tipoPago;
                    var pMonto = cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
                    pMonto.Precision = 18; pMonto.Scale = 2; pMonto.Value = monto;

                    int newId = (int)cmd.ExecuteScalar();
                    MessageBox.Show($"Forma de pago creada. ID = {newId}");
                }
                else
                {
                    const string updateSql = @"
                        UPDATE dbo.FormaPago
                           SET TipoPago = @TipoPago,
                               Monto    = @Monto
                         WHERE PagoID = @PagoID;";

                    using var cmd = new SqlCommand(updateSql, conn);
                    cmd.Parameters.Add("@TipoPago", SqlDbType.NVarChar, 100).Value = tipoPago;
                    var pMonto = cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
                    pMonto.Precision = 18; pMonto.Scale = 2; pMonto.Value = monto;
                    cmd.Parameters.Add("@PagoID", SqlDbType.Int).Value = _selectedPagoId;

                    int affected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Forma de pago actualizada ({affected} registro(s)).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando forma de pago: " + ex.Message);
            }

            LoadFormas();
            ClearFields();
        }

        // Botón Eliminar
        private void button3_Click(object sender, EventArgs e)
        {
            // Asegúrate de tener el ID de la fila seleccionada
            if (!TryGetSelectedPagoId(out int pagoId))
            {
                MessageBox.Show("Selecciona una forma de pago para eliminar.");
                return;
            }

            if (MessageBox.Show("¿Eliminar forma de pago seleccionada?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("DELETE FROM dbo.FormaPago WHERE PagoID = @PagoID", conn);
                cmd.Parameters.Add("@PagoID", SqlDbType.Int).Value = pagoId;
                conn.Open();
                int affected = cmd.ExecuteNonQuery();

                if (affected > 0)
                    MessageBox.Show("Forma de pago eliminada.");
                else
                    MessageBox.Show("No se encontró el registro a eliminar.");
            }
            catch (SqlException ex) when (ex.Number == 547) // FK constraint
            {
                MessageBox.Show("No se puede eliminar: la forma de pago está referenciada en otros registros.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error eliminando forma de pago: " + ex.Message);
            }

            LoadFormas();
            ClearFields();
        }

        // Toma el ID desde la selección (aunque la columna esté oculta)
        private bool TryGetSelectedPagoId(out int pagoId)
        {
            pagoId = 0;
            if (dgvFormas.CurrentRow == null || dgvFormas.CurrentRow.DataBoundItem == null)
                return false;

            if (dgvFormas.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("PagoID"))
                {
                    pagoId = Convert.ToInt32(drv["PagoID"]);
                    _selectedPagoId = pagoId; // sincroniza campo
                    return true;
                }
            }
            else if (dgvFormas.Columns.Contains("PagoID"))
            {
                pagoId = Convert.ToInt32(dgvFormas.CurrentRow.Cells["PagoID"].Value);
                _selectedPagoId = pagoId;
                return true;
            }
            return false;
        }

        // Sincroniza controles al cambiar de fila
        private void dgvFormas_SelectionChanged(object sender, EventArgs e)
        {
            if (!TryGetSelectedPagoId(out int pagoId))
            {
                _selectedPagoId = 0;
                return;
            }

            if (dgvFormas.CurrentRow?.DataBoundItem is DataRowView drv)
            {
                var tipo = drv["TipoPago"]?.ToString();
                if (!string.IsNullOrEmpty(tipo))
                {
                    // Selecciona el item si existe; si no, lo agrega temporalmente
                    if (comboBox1.Items.Contains(tipo)) comboBox1.SelectedItem = tipo;
                    else
                    {
                        comboBox1.Items.Add(tipo);
                        comboBox1.SelectedItem = tipo;
                    }
                }

                if (decimal.TryParse(drv["Monto"]?.ToString(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal m) ||
                    decimal.TryParse(drv["Monto"]?.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out m))
                {
                    textBox1.Text = m.ToString("0.00");
                }
                else
                {
                    textBox1.Text = "0.00";
                }

                lblStatus.Text = $"Editando PagoID={pagoId}";
            }
        }

        // Borrar con tecla Supr
        private void dgvFormas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                button3_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
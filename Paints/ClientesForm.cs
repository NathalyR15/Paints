using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Globalization;


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

                // === Productos ===
                CargarCategorias();                  // llena combo de categoría
                comboBox1.SelectedIndexChanged += (s, ev) => CargarCondicionesPorCategoria(); // dependiente
                CargarGridProductos();               // llena grid de presentaciones
                dgvProductos.AutoGenerateColumns = true;

                // evita errores de combo en grid
                dgvProductos.DataError += (s, ev) => { ev.ThrowException = false; };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inicializando: " + ex.Message);
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
        private void CargarCategorias()
        {
            const string sql = @"SELECT CategoriaID, Nombre FROM dbo.Categoria ORDER BY Nombre;";
            using var conn = new SqlConnection(connectionString);
            using var da = new SqlDataAdapter(sql, conn);
            var dt = new DataTable();
            da.Fill(dt);

            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "CategoriaID";
            comboBox1.DataSource = dt;

            if (dt.Rows.Count > 0)
                CargarCondicionesPorCategoria();
        }

        private void CargarCondicionesPorCategoria()
        {
            if (comboBox1.SelectedValue == null) { comboBox2.DataSource = null; return; }
            int categoriaId = Convert.ToInt32(comboBox1.SelectedValue);

            const string sql = @"
                SELECT 
                    ID_Condicion,
                    CondicionDescripcion AS Condiciones   
                    FROM dbo.Condicion 
                    WHERE CategoriaID = @cat
                    ORDER BY CondicionDescripcion;           
            ";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@cat", System.Data.SqlDbType.Int).Value = categoriaId;

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            comboBox2.DisplayMember = "Condiciones";   // ahora existe en el result set
            comboBox2.ValueMember = "ID_Condicion";
            comboBox2.DataSource = dt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Mapea controles
            string nombreProd = textBox5.Text.Trim();  // txtNombreProd
            string descProd = string.IsNullOrWhiteSpace(textBox6.Text) ? null : textBox6.Text.Trim(); // txtDescripcionProd
            string precioTxt = textBox7.Text.Trim();  // txtPrecioUnitario
            string stockTxt = textBox8.Text.Trim();  // txtStock
            bool estado = checkBox1.Checked;     // chkEstado

            if (comboBox1.SelectedValue == null) { MessageBox.Show("Selecciona una categoría."); return; }
            if (comboBox2.SelectedValue == null) { MessageBox.Show("Selecciona una condición."); return; }
            if (string.IsNullOrWhiteSpace(nombreProd)) { MessageBox.Show("Ingresa el nombre del producto."); return; }
            if (!decimal.TryParse(precioTxt, out var precio) || precio <= 0) { MessageBox.Show("Precio unitario inválido."); return; }
            if (!int.TryParse(stockTxt, out var stock) || stock < 0) { MessageBox.Show("Stock inválido."); return; }

            int categoriaId = Convert.ToInt32(comboBox1.SelectedValue);
            int condicionId = Convert.ToInt32(comboBox2.SelectedValue);

            // descripción de presentación (puedes usar la misma descProd o pedir otra; aquí la calculo)
            // Si tienes un TextBox aparte para la presentación, ponlo aquí.
            string descPresentacion = string.IsNullOrWhiteSpace(textBox9.Text)
                    ? null
                    : textBox9.Text.Trim();

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var tx = conn.BeginTransaction();

                // 1) Insert producto
                const string sqlProd = @"
            INSERT INTO dbo.Producto (Nombre, Descripcion, PrecioUnitario, Stock, CategoriaID)
            VALUES (@n, @d, @p, @s, @cat);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int nuevoProductoId;
                using (var cmd = new SqlCommand(sqlProd, conn, tx))
                {
                    cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 255).Value = nombreProd;
                    cmd.Parameters.Add("@d", System.Data.SqlDbType.NVarChar, -1).Value = (object?)descProd ?? DBNull.Value;
                    cmd.Parameters.Add("@p", System.Data.SqlDbType.Decimal).Value = precio;
                    cmd.Parameters.Add("@s", System.Data.SqlDbType.Int).Value = stock;
                    cmd.Parameters.Add("@cat", System.Data.SqlDbType.Int).Value = categoriaId;
                    nuevoProductoId = (int)cmd.ExecuteScalar();
                }

                // 2) Insert presentación
                const string sqlPres = @"
            INSERT INTO dbo.ProductoPresentacion (ProductoID, ID_Condicion, Estado, Descripcion)
            VALUES (@prod, @cond, @est, @desc);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int nuevaPresentacionId;
                using (var cmd = new SqlCommand(sqlPres, conn, tx))
                {
                    cmd.Parameters.Add("@prod", System.Data.SqlDbType.Int).Value = nuevoProductoId;
                    cmd.Parameters.Add("@cond", System.Data.SqlDbType.Int).Value = condicionId;
                    cmd.Parameters.Add("@est", System.Data.SqlDbType.Bit).Value = estado;
                    cmd.Parameters.Add("@desc", System.Data.SqlDbType.NVarChar, -1) // -1 = NVARCHAR(MAX)
                    .Value = (object?)descPresentacion ?? DBNull.Value;
                    nuevaPresentacionId = (int)cmd.ExecuteScalar();
                }

                tx.Commit();

                MessageBox.Show($"Producto y presentación creados.\nProductoID={nuevoProductoId}, PresentacionID={nuevaPresentacionId}",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormularioProducto();
                CargarGridProductos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error SQL al guardar producto: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar producto: " + ex.Message);
            }

        }
        private void LimpiarFormularioProducto()
        {
            textBox5.Clear(); // nombre
            textBox6.Clear(); // descripcion producto (global)
            textBox7.Clear(); // precio
            textBox8.Clear(); // stock
            checkBox1.Checked = true;
            // deja categoría/condición como están
        }
        private void CargarGridProductos()
        {
            const string sql = @"
        SELECT 
            pp.PresentacionID,
            p.Nombre                         AS Producto,
            ISNULL(p.Descripcion,'')         AS DescripcionProducto,
            ISNULL(pp.Descripcion,'')        AS DescripcionPresentacion,
            c.Nombre                         AS Categoria,
            co.CondicionDescripcion          AS Condicion,     -- 👈 antes: co.Condiciones
            p.PrecioUnitario,
            p.Stock,
            pp.Estado
        FROM dbo.ProductoPresentacion pp
        INNER JOIN dbo.Producto   p  ON p.ProductoID    = pp.ProductoID
        INNER JOIN dbo.Categoria  c  ON c.CategoriaID   = p.CategoriaID
        INNER JOIN dbo.Condicion  co ON co.ID_Condicion = pp.ID_Condicion
        ORDER BY p.Nombre, co.CondicionDescripcion;            -- 👈 antes: co.Condiciones
        ";

            using var conn = new SqlConnection(connectionString);
            using var da = new SqlDataAdapter(sql, conn);
            var dt = new DataTable();
            da.Fill(dt);
            dgvProductos.DataSource = dt;

            if (dgvProductos.Columns.Contains("PresentacionID"))
                dgvProductos.Columns["PresentacionID"].HeaderText = "ID Presentación";
            if (dgvProductos.Columns.Contains("PrecioUnitario"))
                dgvProductos.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";

            //Bloqueo para edicion
            if (dgvProductos.Columns.Contains("PresentacionID"))
                dgvProductos.Columns["PresentacionID"].ReadOnly = true;

            if (dgvProductos.Columns.Contains("Categoria"))
                dgvProductos.Columns["Categoria"].ReadOnly = true;

            if (dgvProductos.Columns.Contains("Condicion"))
                dgvProductos.Columns["Condicion"].ReadOnly = true;

            // Permitir edición solo en estas columnas
            dgvProductos.Columns["Producto"].ReadOnly = false;
            dgvProductos.Columns["DescripcionProducto"].ReadOnly = false;
            dgvProductos.Columns["DescripcionPresentacion"].ReadOnly = false;
            dgvProductos.Columns["PrecioUnitario"].ReadOnly = false;
            dgvProductos.Columns["Stock"].ReadOnly = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para editar.");
                return;
            }

            this.Validate();
            dgvProductos.EndEdit();
            dgvProductos.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (!int.TryParse(dgvProductos.CurrentRow.Cells["PresentacionID"]?.Value?.ToString(), out var presentacionId))
            {
                MessageBox.Show("ID de presentación inválido.");
                return;
            }

            string nombre = dgvProductos.CurrentRow.Cells["Producto"]?.Value?.ToString()?.Trim() ?? "";
            string descProd = dgvProductos.CurrentRow.Cells["DescripcionProducto"]?.Value?.ToString()?.Trim() ?? "";
            string descPres = dgvProductos.CurrentRow.Cells["DescripcionPresentacion"]?.Value?.ToString()?.Trim() ?? ""; // ✅

            if (!decimal.TryParse(
                    dgvProductos.CurrentRow.Cells["PrecioUnitario"]?.Value?.ToString(),
                    NumberStyles.Number, CultureInfo.InvariantCulture, out var precio) || precio < 0)
            {
                MessageBox.Show("Ingrese un precio válido (>= 0).");
                return;
            }

            if (!int.TryParse(
                    dgvProductos.CurrentRow.Cells["Stock"]?.Value?.ToString(),
                    out var stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock válido (>= 0).");
                return;
            }

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var tx = conn.BeginTransaction();

                const string sqlProducto = @"
                    UPDATE p
                       SET p.Nombre         = @n,
                           p.Descripcion    = @dp,
                           p.PrecioUnitario = @pu,
                           p.Stock          = @st
                    FROM dbo.Producto p
                    INNER JOIN dbo.ProductoPresentacion pp ON pp.ProductoID = p.ProductoID
                    WHERE pp.PresentacionID = @pres;";

                using (var cmd = new SqlCommand(sqlProducto, conn, tx))
                {
                    cmd.Parameters.Add("@n", System.Data.SqlDbType.NVarChar, 255).Value = (object)nombre ?? DBNull.Value;
                    cmd.Parameters.Add("@dp", System.Data.SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(descProd) ? DBNull.Value : descProd;

                    var pPrecio = cmd.Parameters.Add("@pu", System.Data.SqlDbType.Decimal);
                    pPrecio.Precision = 18; pPrecio.Scale = 2; pPrecio.Value = precio;

                    cmd.Parameters.Add("@st", System.Data.SqlDbType.Int).Value = stock;
                    cmd.Parameters.Add("@pres", System.Data.SqlDbType.Int).Value = presentacionId;

                    cmd.ExecuteNonQuery();
                }

                const string sqlPresentacion = @"
                    UPDATE dbo.ProductoPresentacion
                       SET Descripcion = @desc
                     WHERE PresentacionID = @pres;";

                using (var cmd = new SqlCommand(sqlPresentacion, conn, tx))
                {
                    cmd.Parameters.Add("@desc", System.Data.SqlDbType.NVarChar, 255).Value =
                        string.IsNullOrWhiteSpace(descPres) ? DBNull.Value : descPres;
                    cmd.Parameters.Add("@pres", System.Data.SqlDbType.Int).Value = presentacionId;

                    cmd.ExecuteNonQuery();
                }

                tx.Commit();

                MessageBox.Show("Cambios guardados.");
                CargarGridProductos(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
                return;
            }

            if (!int.TryParse(dgvProductos.CurrentRow.Cells["PresentacionID"]?.Value?.ToString(), out int presentacionId))
            {
                MessageBox.Show("No se pudo obtener el ID de presentación.");
                return;
            }


            var resp = MessageBox.Show(
                "¿Eliminar esta presentación? (Si el producto queda sin presentaciones y no tiene ventas, también se eliminará.)",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resp != DialogResult.Yes) return;

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var tx = conn.BeginTransaction();

        
                int productoId;
                using (var cmd = new SqlCommand(
                    "SELECT ProductoID FROM dbo.ProductoPresentacion WHERE PresentacionID = @pres;",
                    conn, tx))
                {
                    cmd.Parameters.Add("@pres", System.Data.SqlDbType.Int).Value = presentacionId;
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value)
                    {
                        MessageBox.Show("La presentación ya no existe.");
                        tx.Rollback();
                        CargarGridProductos();
                        return;
                    }
                    productoId = Convert.ToInt32(obj);
                }

               
                using (var cmd = new SqlCommand(
                    "DELETE FROM dbo.ProductoPresentacion WHERE PresentacionID = @pres;",
                    conn, tx))
                {
                    cmd.Parameters.Add("@pres", System.Data.SqlDbType.Int).Value = presentacionId;
                    cmd.ExecuteNonQuery();
                }

              
                bool quedanPresentaciones;
                using (var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM dbo.ProductoPresentacion WHERE ProductoID = @prod;",
                    conn, tx))
                {
                    cmd.Parameters.Add("@prod", System.Data.SqlDbType.Int).Value = productoId;
                    quedanPresentaciones = (int)cmd.ExecuteScalar() > 0;
                }

                if (!quedanPresentaciones)
                {
                    bool tieneVentas;
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(1) FROM dbo.DetalleFactura WHERE ProductoID = @prod;",
                        conn, tx))
                    {
                        cmd.Parameters.Add("@prod", System.Data.SqlDbType.Int).Value = productoId;
                        tieneVentas = (int)cmd.ExecuteScalar() > 0;
                    }

                    if (!tieneVentas)
                    {
                        using var cmdDelProd = new SqlCommand(
                            "DELETE FROM dbo.Producto WHERE ProductoID = @prod;",
                            conn, tx);
                        cmdDelProd.Parameters.Add("@prod", System.Data.SqlDbType.Int).Value = productoId;
                        cmdDelProd.ExecuteNonQuery();
                    }
                }

                tx.Commit();

                MessageBox.Show("Eliminación realizada correctamente.");
                CargarGridProductos(); 
            }
            catch (SqlException ex) when (ex.Number == 547) 
            {
                MessageBox.Show(
                    "No se puede eliminar porque está referenciado por otros registros (por ejemplo, facturas).",
                    "Operación no permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }
    }
}
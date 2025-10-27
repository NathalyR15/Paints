using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Capa_de_datos;
using Capa_de_servicios;             
using Capa_de_servicios.Services;    

namespace Paints
{
    public partial class FacturacionForm : Form
    {
        private readonly FacturaService _facturaService = new FacturaService();
        private readonly ProductoService _productoService = new ProductoService();
        private readonly ClienteService _clienteService = new ClienteService();
        private readonly FormaPagoService _formaPagoService = new FormaPagoService();
        private int? _ultimaFacturaId;
        private readonly List<PagoDto> _pagos = new List<PagoDto>();

        public FacturacionForm()
        {
            InitializeComponent();
            this.Load += FacturacionForm_Load;
        }

        private void FacturacionForm_Load(object sender, EventArgs e)
        {
            InicializarUI();
        }

        private void InicializarUI()
        {
            InicializarDataGridView();

            numericUpDownCantidad.DecimalPlaces = 2;
            numericUpDownCantidad.Minimum = 0.01M;
            numericUpDownCantidad.Value = 1.00M;

            lblSubtotal.Text = "0.00";
            lblTotal.Text = "0.00";
            lblTotalPagado.Text = "0.00";
            lblSaldo.Text = "0.00";

            CargarClientes();
            CargarCategorias();
            CargarFormasPago(); 
        }

        private void InicializarDataGridView()
        {
            dgvFacturaDetalles.Columns.Clear();
            dgvFacturaDetalles.AutoGenerateColumns = false;
            dgvFacturaDetalles.AllowUserToAddRows = false;

            dgvFacturaDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProductoID", HeaderText = "ProductoID", DataPropertyName = "ProductoID", Visible = false });
            dgvFacturaDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Producto", DataPropertyName = "Nombre", Width = 250 });
            dgvFacturaDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cantidad", HeaderText = "Cantidad", DataPropertyName = "Cantidad" });
            dgvFacturaDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrecioUnitario", HeaderText = "Precio Unit.", DataPropertyName = "PrecioUnitario" });
            dgvFacturaDetalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "SubTotal", HeaderText = "SubTotal", DataPropertyName = "SubTotal" });

            dgvPagos.Columns.Clear();
            dgvPagos.AutoGenerateColumns = false;
            dgvPagos.Columns.Add(new DataGridViewTextBoxColumn { Name = "TipoPago", HeaderText = "TipoPago", DataPropertyName = "TipoPago" });
            dgvPagos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Monto", HeaderText = "Monto", DataPropertyName = "Monto" });
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteService.GetAll() ?? new List<ClienteDAL>();
                if (clientes.Count > 0)
                {
                    var prop = clientes[0].GetType().GetProperty("NombreCompleto");
                    comboBoxCliente.DisplayMember = (prop != null) ? "NombreCompleto" : "Nombre";
                }
                comboBoxCliente.ValueMember = "ClienteID";
                comboBoxCliente.DataSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando clientes: " + ex.Message);
            }
        }

        private void CargarCategorias()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var categorias = db.Categoria.OrderBy(c => c.Nombre).ToList();
                comboBoxCategoria.DisplayMember = "Nombre";
                comboBoxCategoria.ValueMember = "CategoriaID";
                comboBoxCategoria.DataSource = categorias;
                if (categorias.Count > 0) comboBoxCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando categorías: " + ex.Message);
            }
        }

        private void CargarFormasPago()
        {
            try
            {
              
                var formas = _formaPagoService.GetAll();

                // Si por alguna razón GetAll() devuelve null (defensivo), lo reemplazamos por lista vacía
                if (formas == null)
                    formas = new List<FormaPago>();

                comboBoxFormaPago.DisplayMember = "TipoPago";
                comboBoxFormaPago.ValueMember = "PagoID";
                comboBoxFormaPago.DataSource = formas;

                if (formas.Count > 0)
                    comboBoxFormaPago.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando formas de pago: " + ex.Message);
            }

        }

        // ********** BUSCAR PRODUCTOS POR CATEGORIA **********
        private void buttonBuscarProducto_Click_1(object sender, EventArgs e)
        {
            BuscarPorCategoria();
        }

        private void BuscarPorCategoria()
        {
            try
            {
                if (comboBoxCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione una categoría.");
                    return;
                }

                int categoriaId;
                if (comboBoxCategoria.SelectedValue is int iv) categoriaId = iv;
                else if (!int.TryParse(comboBoxCategoria.SelectedValue.ToString(), out categoriaId))
                {
                    MessageBox.Show("Id de categoría inválido.");
                    return;
                }

                var productos = _productoService.GetByCategoria(categoriaId) ?? new List<Producto>();

                var listaParaCombo = productos
                    .Select(p => new { ProductoID = p.ProductoID, Nombre = p.Nombre ?? string.Empty, PrecioUnitario = p.PrecioUnitario, Stock = p.Stock })
                    .ToList();

                comboBoxProductosEncontrados.DataSource = null;
                comboBoxProductosEncontrados.DisplayMember = "Nombre";
                comboBoxProductosEncontrados.ValueMember = "ProductoID";
                comboBoxProductosEncontrados.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBoxProductosEncontrados.DataSource = listaParaCombo;

                if (listaParaCombo.Count == 0)
                    MessageBox.Show("No se encontraron productos para la categoría seleccionada.");
                else
                    comboBoxProductosEncontrados.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos por categoría: " + ex.Message);
            }
        }

        // ********** AGREGAR DETALLE **********
        private void buttonAgregarDetalle_Click_1(object sender, EventArgs e)
        {
            AgregarDetalle();
        }

        private void AgregarDetalle()
        {
            if (comboBoxProductosEncontrados.DataSource == null)
            {
                MessageBox.Show("Primero busque y seleccione un producto.");
                return;
            }

            var selected = comboBoxProductosEncontrados.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Seleccione un producto antes de agregarlo.");
                return;
            }

            int productoId = 0;
            string nombre = "";
            object? precioObj = null;

            var t = selected.GetType();
            var propId = t.GetProperty("ProductoID");
            var propNombre = t.GetProperty("Nombre");
            var propPrecio = t.GetProperty("PrecioUnitario");
            if (propId != null) int.TryParse(propId.GetValue(selected)?.ToString(), out productoId);
            if (propNombre != null) nombre = propNombre.GetValue(selected)?.ToString() ?? "";
            if (propPrecio != null) precioObj = propPrecio.GetValue(selected);

            if (productoId == 0)
            {
                MessageBox.Show("ID de producto inválido.");
                return;
            }

            decimal cantidad = numericUpDownCantidad.Value;
            if (cantidad <= 0) { MessageBox.Show("Cantidad inválida."); return; }

            if (!TryGetDecimalFromObject(precioObj, out decimal precio))
            {
                MessageBox.Show("Precio del producto inválido o tipo inesperado.");
                return;
            }

            decimal subtotal = cantidad * precio;
            dgvFacturaDetalles.Rows.Add(productoId, nombre, cantidad, precio, subtotal);
            RecalcularTotales();
        }

        private bool TryGetDecimalFromObject(object? value, out decimal result)
        {
            result = 0m;
            if (value == null || value == DBNull.Value) return false;
            if (value is decimal d) { result = d; return true; }
            if (value is int i) { result = Convert.ToDecimal(i); return true; }
            if (value is long l) { result = Convert.ToDecimal(l); return true; }
            if (value is double db) { result = Convert.ToDecimal(db); return true; }
            if (value is float f) { result = Convert.ToDecimal(f); return true; }
            if (value is string s && decimal.TryParse(s, out var parsed)) { result = parsed; return true; }
            try { result = Convert.ToDecimal(value); return true; } catch { return false; }
        }

        private void buttonQuitarDetalle_Click_1(object sender, EventArgs e)
        {
            if (dgvFacturaDetalles.CurrentRow != null)
            {
                dgvFacturaDetalles.Rows.Remove(dgvFacturaDetalles.CurrentRow);
                RecalcularTotales();
            }
        }

        // ********** AGREGAR PAGO (usa SelectedValue para PagoID) **********
        private void buttonAgregarPago_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMontoPago.Text) || !decimal.TryParse(textBoxMontoPago.Text.Trim(), out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto válido mayor a 0.");
                return;
            }

            if (comboBoxFormaPago.SelectedValue == null)
            {
                MessageBox.Show("Seleccione forma de pago.");
                return;
            }

            int pagoId;
            if (comboBoxFormaPago.SelectedValue is int pv) pagoId = pv;
            else if (!int.TryParse(comboBoxFormaPago.SelectedValue.ToString(), out pagoId))
            {
                MessageBox.Show("Forma de pago inválida.");
                return;
            }

            var dto = new PagoDto { PagoID = pagoId, MontoPago = monto };

            _pagos.Add(dto);
            // Mostrar descripción visible en grid (tipo) y monto
            string tipoTexto = comboBoxFormaPago.Text ?? pagoId.ToString();
            dgvPagos.Rows.Add(tipoTexto, monto);

            textBoxMontoPago.Clear();
            RecalcularTotales();
        }

        // ********** GUARDAR FACTURA (call al servicio) **********
        private void buttonGuardarFactura_Click_1(object sender, EventArgs e)
        {
            GuardarFactura();
        }

        private void GuardarFactura()
        {
            if (comboBoxCliente.SelectedValue == null)
            {
                MessageBox.Show("Seleccione cliente.");
                return;
            }

            if (dgvFacturaDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto en la factura.");
                return;
            }

            var dto = new FacturaDto
            {
                ClienteID = Convert.ToInt32(comboBoxCliente.SelectedValue),
                UsuarioID = 1 // reemplazar por usuario logueado
            };

            // Detalles
            foreach (DataGridViewRow r in dgvFacturaDetalles.Rows)
            {
                if (r.IsNewRow) continue;
                if (r.Cells["ProductoID"].Value == null) continue;

                var prodCell = r.Cells["ProductoID"].Value;
                var cantCell = r.Cells["Cantidad"].Value;
                var precioCell = r.Cells["PrecioUnitario"].Value;

                if (!int.TryParse(prodCell?.ToString(), out int productoId))
                {
                    MessageBox.Show("Producto ID inválido en detalle.");
                    return;
                }
                if (!TryGetDecimalFromObject(cantCell, out decimal cantidad))
                {
                    MessageBox.Show("Cantidad inválida en detalle.");
                    return;
                }
                if (!TryGetDecimalFromObject(precioCell, out decimal precioUnitario))
                {
                    MessageBox.Show("Precio inválido en detalle.");
                    return;
                }

                dto.Detalles.Add(new FacturaDetalleDto
                {
                    ProductoID = productoId,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario
                });
            }

            // Pagos
            foreach (var p in _pagos) dto.Pagos.Add(p);

            try
            {
                int newFacturaId = _facturaService.CreateFactura(dto);

                _ultimaFacturaId = newFacturaId;               
                     

                MessageBox.Show($"Factura guardada. ID = {newFacturaId}");


                _pagos.Clear();
                RecalcularTotales();
            }
            catch (Exception ex)
            {
                // muestra inner exception si existe (muy útil)
                var inner = ex.InnerException != null ? ex.InnerException.Message : null;
                MessageBox.Show("Error guardando factura: " + (inner ?? ex.Message));
            }
        }

        // ********** IMPRIMIR (vaucher) **********
        private void buttonImprimir_Click_1(object sender, EventArgs e)
        {

            ImprimirFacturaPreview(); // opcional: pasar id si la factura ya fue guardada
        }

        private void ImprimirFacturaPreview(int? facturaId = null)
        {

            // intento de cargar factura con detalle si se pasó id (si tu servicio tiene ese método)
            Factura factura = null;
            if (!facturaId.HasValue && _ultimaFacturaId.HasValue)
                facturaId = _ultimaFacturaId;

            var pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                float y = 20;
                var g = ev.Graphics;
                if (g == null) return;

                // encabezado
                g.DrawString("FACTURA", new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 20, y);
                y += 26;
                g.DrawString($"No. Factura: {(factura != null ? factura.FacturaID.ToString() : (facturaId.HasValue ? facturaId.Value.ToString() : "N/A"))}",
                             new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 20, y);
                g.DrawString($"Fecha: {(factura != null ? factura.Fecha.ToString("g") : DateTime.Now.ToString("g"))}",
                             new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 360, y);
                y += 24;
                g.DrawString($"Cliente: {(factura?.Cliente?.Nombre ?? comboBoxCliente.Text)}", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 20, y);
                y += 20;

                // títulos
                g.DrawString("Cantidad", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 20, y);
                g.DrawString("Producto", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 100, y);
                g.DrawString("Precio Unit.", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 360, y);
                g.DrawString("SubTotal", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 460, y);
                y += 18;

                if (factura != null && factura.DetalleFactura != null && factura.DetalleFactura.Any())
                {
                    foreach (var d in factura.DetalleFactura)
                    {
                        string prodNombre = d.Producto?.Nombre ?? d.ProductoID.ToString();
                        g.DrawString(d.Cantidad.ToString("0.##"), new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 20, y);
                        g.DrawString(prodNombre, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 100, y);
                        g.DrawString(d.PrecioUnitario.ToString("N2"), new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 360, y);
                        g.DrawString(d.SubTotal.ToString("N2"), new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 460, y);
                        y += 18;
                    }
                }
                else
                {
                    foreach (DataGridViewRow r in dgvFacturaDetalles.Rows)
                    {
                        if (r.IsNewRow) continue;
                        string nombre = r.Cells["Nombre"].Value?.ToString() ?? "";
                        string cantidad = r.Cells["Cantidad"].Value?.ToString() ?? "0";
                        string precio = r.Cells["PrecioUnitario"].Value?.ToString() ?? "0.00";
                        string subtotal = r.Cells["SubTotal"].Value?.ToString() ?? "0.00";

                        g.DrawString(cantidad, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 20, y);
                        g.DrawString(nombre, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 100, y);
                        g.DrawString(precio, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 360, y);
                        g.DrawString(subtotal, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 460, y);
                        y += 18;
                    }
                }

                y += 12;
                g.DrawString("Total: " + lblTotal.Text, new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 20, y);
            };
            
            using var dlg = new PrintPreviewDialog { Document = pd, Width = 800, Height = 600 };
            dlg.ShowDialog();

            dgvFacturaDetalles.Rows.Clear();
            dgvPagos.Rows.Clear();
        }


        // recalcula totales
        private void RecalcularTotales()
        {
            decimal subtotal = 0;
            foreach (DataGridViewRow r in dgvFacturaDetalles.Rows)
            {
                if (r.Cells["SubTotal"].Value != null)
                {
                    if (decimal.TryParse(r.Cells["SubTotal"].Value.ToString(), out var val))
                        subtotal += val;
                }
            }

            lblSubtotal.Text = subtotal.ToString("0.00");
            decimal total = subtotal;
            lblTotal.Text = total.ToString("0.00");

            decimal pagado = _pagos.Sum(p => p.MontoPago);
            lblTotalPagado.Text = pagado.ToString("0.00");

            lblSaldo.Text = (total - pagado).ToString("0.00");
        }

        // event handlers vacíos (mantener)
        private void comboBoxProductosEncontrados_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBoxCategoria_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBoxCliente_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBoxFormaPago_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}

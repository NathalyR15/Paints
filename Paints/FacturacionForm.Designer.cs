namespace Paints
{
    partial class FacturacionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBoxCliente = new ComboBox();
            dgvFacturaDetalles = new DataGridView();
            lblSubtotal = new Label();
            lblTotal = new Label();
            lblTotalPagado = new Label();
            comboBoxProductosEncontrados = new ComboBox();
            buttonAgregarDetalle = new Button();
            buttonQuitarDetalle = new Button();
            label4 = new Label();
            buttonGuardarFactura = new Button();
            buttonImprimir = new Button();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            numericUpDownCantidad = new NumericUpDown();
            label8 = new Label();
            comboBoxFormaPago = new ComboBox();
            label1 = new Label();
            textBoxMontoPago = new TextBox();
            buttonAgregarPago = new Button();
            label2 = new Label();
            dgvPagos = new DataGridView();
            lblSaldo = new Label();
            buttonBuscarProducto = new Button();
            comboBoxCategoria = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvFacturaDetalles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
            SuspendLayout();
            // 
            // comboBoxCliente
            // 
            comboBoxCliente.FormattingEnabled = true;
            comboBoxCliente.Location = new Point(161, 9);
            comboBoxCliente.Name = "comboBoxCliente";
            comboBoxCliente.Size = new Size(151, 28);
            comboBoxCliente.TabIndex = 0;
            comboBoxCliente.SelectedIndexChanged += comboBoxCliente_SelectedIndexChanged;
            // 
            // dgvFacturaDetalles
            // 
            dgvFacturaDetalles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFacturaDetalles.Location = new Point(12, 152);
            dgvFacturaDetalles.Name = "dgvFacturaDetalles";
            dgvFacturaDetalles.RowHeadersWidth = 51;
            dgvFacturaDetalles.Size = new Size(490, 188);
            dgvFacturaDetalles.TabIndex = 3;
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(27, 369);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(50, 20);
            lblSubtotal.TabIndex = 4;
            lblSubtotal.Text = "label1";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(129, 369);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(50, 20);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "label2";
            // 
            // lblTotalPagado
            // 
            lblTotalPagado.AutoSize = true;
            lblTotalPagado.Location = new Point(627, 525);
            lblTotalPagado.Name = "lblTotalPagado";
            lblTotalPagado.Size = new Size(50, 20);
            lblTotalPagado.TabIndex = 6;
            lblTotalPagado.Text = "label3";
            // 
            // comboBoxProductosEncontrados
            // 
            comboBoxProductosEncontrados.FormattingEnabled = true;
            comboBoxProductosEncontrados.Location = new Point(413, 54);
            comboBoxProductosEncontrados.Name = "comboBoxProductosEncontrados";
            comboBoxProductosEncontrados.Size = new Size(151, 28);
            comboBoxProductosEncontrados.TabIndex = 7;
            comboBoxProductosEncontrados.SelectedIndexChanged += comboBoxProductosEncontrados_SelectedIndexChanged;
            // 
            // buttonAgregarDetalle
            // 
            buttonAgregarDetalle.Location = new Point(508, 179);
            buttonAgregarDetalle.Name = "buttonAgregarDetalle";
            buttonAgregarDetalle.Size = new Size(94, 32);
            buttonAgregarDetalle.TabIndex = 8;
            buttonAgregarDetalle.Text = "Agregar";
            buttonAgregarDetalle.UseVisualStyleBackColor = true;
            buttonAgregarDetalle.Click += buttonAgregarDetalle_Click_1;
            // 
            // buttonQuitarDetalle
            // 
            buttonQuitarDetalle.Location = new Point(508, 231);
            buttonQuitarDetalle.Name = "buttonQuitarDetalle";
            buttonQuitarDetalle.Size = new Size(94, 32);
            buttonQuitarDetalle.TabIndex = 9;
            buttonQuitarDetalle.Text = "Quitar";
            buttonQuitarDetalle.UseVisualStyleBackColor = true;
            buttonQuitarDetalle.Click += buttonQuitarDetalle_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(508, 144);
            label4.Name = "label4";
            label4.Size = new Size(63, 20);
            label4.TabIndex = 10;
            label4.Text = "Detalles";
            // 
            // buttonGuardarFactura
            // 
            buttonGuardarFactura.Location = new Point(12, 435);
            buttonGuardarFactura.Name = "buttonGuardarFactura";
            buttonGuardarFactura.Size = new Size(94, 29);
            buttonGuardarFactura.TabIndex = 11;
            buttonGuardarFactura.Text = "Guardar";
            buttonGuardarFactura.UseVisualStyleBackColor = true;
            buttonGuardarFactura.Click += buttonGuardarFactura_Click_1;
            // 
            // buttonImprimir
            // 
            buttonImprimir.Location = new Point(161, 455);
            buttonImprimir.Name = "buttonImprimir";
            buttonImprimir.Size = new Size(94, 29);
            buttonImprimir.TabIndex = 12;
            buttonImprimir.Text = "Imprimir";
            buttonImprimir.UseVisualStyleBackColor = true;
            buttonImprimir.Click += buttonImprimir_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(530, 310);
            label5.Name = "label5";
            label5.Size = new Size(56, 20);
            label5.TabIndex = 13;
            label5.Text = "Factura";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 17);
            label6.Name = "label6";
            label6.Size = new Size(129, 20);
            label6.TabIndex = 14;
            label6.Text = "1. Lista de clientes";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 60);
            label7.Name = "label7";
            label7.Size = new Size(128, 20);
            label7.TabIndex = 15;
            label7.Text = "2.Buscar producto";
            // 
            // numericUpDownCantidad
            // 
            numericUpDownCantidad.Location = new Point(661, 53);
            numericUpDownCantidad.Name = "numericUpDownCantidad";
            numericUpDownCantidad.Size = new Size(150, 27);
            numericUpDownCantidad.TabIndex = 16;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(573, 60);
            label8.Name = "label8";
            label8.Size = new Size(69, 20);
            label8.TabIndex = 17;
            label8.Text = "Cantidad";
            // 
            // comboBoxFormaPago
            // 
            comboBoxFormaPago.FormattingEnabled = true;
            comboBoxFormaPago.Items.AddRange(new object[] { "Efectivo", "Cheque", "Tarjeta" });
            comboBoxFormaPago.Location = new Point(934, 48);
            comboBoxFormaPago.Name = "comboBoxFormaPago";
            comboBoxFormaPago.Size = new Size(151, 28);
            comboBoxFormaPago.TabIndex = 18;
            comboBoxFormaPago.SelectedIndexChanged += comboBoxFormaPago_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(819, 55);
            label1.Name = "label1";
            label1.Size = new Size(111, 20);
            label1.TabIndex = 19;
            label1.Text = "Forma de pago";
            // 
            // textBoxMontoPago
            // 
            textBoxMontoPago.Location = new Point(802, 108);
            textBoxMontoPago.Name = "textBoxMontoPago";
            textBoxMontoPago.Size = new Size(125, 27);
            textBoxMontoPago.TabIndex = 20;
            // 
            // buttonAgregarPago
            // 
            buttonAgregarPago.Location = new Point(975, 108);
            buttonAgregarPago.Name = "buttonAgregarPago";
            buttonAgregarPago.Size = new Size(94, 29);
            buttonAgregarPago.TabIndex = 21;
            buttonAgregarPago.Text = "Agregar";
            buttonAgregarPago.UseVisualStyleBackColor = true;
            buttonAgregarPago.Click += buttonAgregarPago_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(677, 111);
            label2.Name = "label2";
            label2.Size = new Size(110, 20);
            label2.TabIndex = 22;
            label2.Text = "Ingresar monto";
            // 
            // dgvPagos
            // 
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Location = new Point(627, 334);
            dgvPagos.Name = "dgvPagos";
            dgvPagos.RowHeadersWidth = 51;
            dgvPagos.Size = new Size(442, 188);
            dgvPagos.TabIndex = 23;
            // 
            // lblSaldo
            // 
            lblSaldo.AutoSize = true;
            lblSaldo.Location = new Point(761, 525);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Size = new Size(50, 20);
            lblSaldo.TabIndex = 24;
            lblSaldo.Text = "label3";
            // 
            // buttonBuscarProducto
            // 
            buttonBuscarProducto.Location = new Point(308, 55);
            buttonBuscarProducto.Name = "buttonBuscarProducto";
            buttonBuscarProducto.Size = new Size(94, 29);
            buttonBuscarProducto.TabIndex = 25;
            buttonBuscarProducto.Text = "Buscar";
            buttonBuscarProducto.UseVisualStyleBackColor = true;
            buttonBuscarProducto.Click += buttonBuscarProducto_Click_1;
            // 
            // comboBoxCategoria
            // 
            comboBoxCategoria.FormattingEnabled = true;
            comboBoxCategoria.Location = new Point(151, 60);
            comboBoxCategoria.Name = "comboBoxCategoria";
            comboBoxCategoria.Size = new Size(151, 28);
            comboBoxCategoria.TabIndex = 26;
            comboBoxCategoria.SelectedIndexChanged += comboBoxCategoria_SelectedIndexChanged;
            // 
            // FacturacionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1155, 554);
            Controls.Add(comboBoxCategoria);
            Controls.Add(buttonBuscarProducto);
            Controls.Add(lblSaldo);
            Controls.Add(dgvPagos);
            Controls.Add(label2);
            Controls.Add(buttonAgregarPago);
            Controls.Add(textBoxMontoPago);
            Controls.Add(label1);
            Controls.Add(comboBoxFormaPago);
            Controls.Add(label8);
            Controls.Add(numericUpDownCantidad);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(buttonImprimir);
            Controls.Add(buttonGuardarFactura);
            Controls.Add(label4);
            Controls.Add(buttonQuitarDetalle);
            Controls.Add(buttonAgregarDetalle);
            Controls.Add(comboBoxProductosEncontrados);
            Controls.Add(lblTotalPagado);
            Controls.Add(lblTotal);
            Controls.Add(lblSubtotal);
            Controls.Add(dgvFacturaDetalles);
            Controls.Add(comboBoxCliente);
            Name = "FacturacionForm";
            Text = "FacturacionForm";
            ((System.ComponentModel.ISupportInitialize)dgvFacturaDetalles).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxCliente;
        private DataGridView dgvFacturaDetalles;
        private Label lblSubtotal;
        private Label lblTotal;
        private Label lblTotalPagado;
        private ComboBox comboBoxProductosEncontrados;
        private Button buttonAgregarDetalle;
        private Button buttonQuitarDetalle;
        private Label label4;
        private Button buttonGuardarFactura;
        private Button buttonImprimir;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericUpDown numericUpDownCantidad;
        private Label label8;
        private ComboBox comboBoxFormaPago;
        private Label label1;
        private TextBox textBoxMontoPago;
        private Button buttonAgregarPago;
        private Label label2;
        private DataGridView dgvPagos;
        private Label lblSaldo;
        private Button buttonBuscarProducto;
        private ComboBox comboBoxCategoria;
    }
}
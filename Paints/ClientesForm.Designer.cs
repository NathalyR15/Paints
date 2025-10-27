namespace Paints
{
    partial class ClientesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientesForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            dgvClientes = new DataGridView();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            lblStatus = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            comboBox1 = new ComboBox();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            textBox7 = new TextBox();
            textBox8 = new TextBox();
            comboBox2 = new ComboBox();
            label12 = new Label();
            textBox9 = new TextBox();
            label13 = new Label();
            checkBox1 = new CheckBox();
            btnGuardar = new Button();
            dgvProductos = new DataGridView();
            button4 = new Button();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 67);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 0;
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(53, 103);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 1;
            label2.Text = "Apellido";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(53, 142);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 2;
            label3.Text = "Telefono";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(53, 182);
            label4.Name = "label4";
            label4.Size = new Size(54, 20);
            label4.TabIndex = 3;
            label4.Text = "Correo";
            // 
            // dgvClientes
            // 
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Location = new Point(12, 279);
            dgvClientes.MultiSelect = false;
            dgvClientes.Name = "dgvClientes";
            dgvClientes.RowHeadersWidth = 51;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(474, 273);
            dgvClientes.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(180, 60);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(180, 96);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 6;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(180, 135);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(180, 175);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(125, 27);
            textBox4.TabIndex = 8;
            // 
            // button1
            // 
            button1.Location = new Point(381, 63);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 9;
            button1.Text = "Nuevo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(381, 121);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 10;
            button2.Text = "Guardar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(381, 173);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 11;
            button3.Text = "Eliminar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(216, 242);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "label5";
            lblStatus.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(247, 15);
            label5.Name = "label5";
            label5.Size = new Size(122, 20);
            label5.TabIndex = 13;
            label5.Text = "Registrar clientes";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(749, 15);
            label6.Name = "label6";
            label6.Size = new Size(134, 20);
            label6.TabIndex = 14;
            label6.Text = "Registar productos";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(562, 44);
            label7.Name = "label7";
            label7.Size = new Size(149, 20);
            label7.TabIndex = 15;
            label7.Text = "Seleccione Categoria";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Accesorios", "Solventes", "Pinturas", "Barnices" });
            comboBox1.Location = new Point(737, 41);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 16;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(615, 94);
            label8.Name = "label8";
            label8.Size = new Size(64, 20);
            label8.TabIndex = 17;
            label8.Text = "Nombre";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(615, 129);
            label9.Name = "label9";
            label9.Size = new Size(87, 20);
            label9.TabIndex = 18;
            label9.Text = "Descripcion";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(615, 162);
            label10.Name = "label10";
            label10.Size = new Size(105, 20);
            label10.TabIndex = 19;
            label10.Text = "Precio unitario";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(615, 195);
            label11.Name = "label11";
            label11.Size = new Size(45, 20);
            label11.TabIndex = 20;
            label11.Text = "Stock";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(737, 87);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(125, 27);
            textBox5.TabIndex = 21;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(737, 122);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(125, 27);
            textBox6.TabIndex = 22;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(737, 155);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(125, 27);
            textBox7.TabIndex = 23;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(737, 188);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(125, 27);
            textBox8.TabIndex = 24;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(732, 234);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(151, 28);
            comboBox2.TabIndex = 25;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(615, 242);
            label12.Name = "label12";
            label12.Size = new Size(76, 20);
            label12.TabIndex = 26;
            label12.Text = "Condicion";
            // 
            // textBox9
            // 
            textBox9.Location = new Point(906, 66);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(202, 27);
            textBox9.TabIndex = 27;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(925, 42);
            label13.Name = "label13";
            label13.Size = new Size(183, 20);
            label13.TabIndex = 28;
            label13.Text = "Presentacion del producto";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(972, 115);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(76, 24);
            checkBox1.TabIndex = 29;
            checkBox1.Text = "Estado";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(954, 145);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(94, 29);
            btnGuardar.TabIndex = 30;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // dgvProductos
            // 
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(509, 323);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.Size = new Size(570, 229);
            dgvProductos.TabIndex = 31;
            // 
            // button4
            // 
            button4.Location = new Point(954, 195);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 32;
            button4.Text = "Editar";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(954, 242);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 33;
            button5.Text = "Eliminar";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // ClientesForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1118, 578);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(dgvProductos);
            Controls.Add(btnGuardar);
            Controls.Add(checkBox1);
            Controls.Add(label13);
            Controls.Add(textBox9);
            Controls.Add(label12);
            Controls.Add(comboBox2);
            Controls.Add(textBox8);
            Controls.Add(textBox7);
            Controls.Add(textBox6);
            Controls.Add(textBox5);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(comboBox1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(lblStatus);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(dgvClientes);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ClientesForm";
            Text = "ClientesForm";
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private DataGridView dgvClientes;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label lblStatus;
        private Label label5;
        private Label label6;
        private Label label7;
        private ComboBox comboBox1;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private ComboBox comboBox2;
        private Label label12;
        private TextBox textBox9;
        private Label label13;
        private CheckBox checkBox1;
        private Button btnGuardar;
        private DataGridView dgvProductos;
        private Button button4;
        private Button button5;
    }
}
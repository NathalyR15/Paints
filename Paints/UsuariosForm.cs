using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Capa_de_datos;
using Microsoft.EntityFrameworkCore;

namespace Paints
{
    public partial class UsuariosForm : Form
    {
       
        private DataTable _dtUsuarios;
        private BindingSource _bsUsuarios = new BindingSource();

        private string connectionString = "Data Source=NATH;Initial Catalog=PaintsDB;Integrated Security=True;TrustServerCertificate=True;";

        public UsuariosForm()
        {
            InitializeComponent();

            // Evita que el DataGridView explote si el combo ve un valor no presente
            dgvUsuarios.DataError += (s, e) => { e.ThrowException = false; };

            // Si estás editando un ComboBox, commit inmediato
            dgvUsuarios.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvUsuarios.IsCurrentCellDirty)
                    dgvUsuarios.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using var db = DbContextFactory.Create();

                var data = (
                    from u in db.Usuario
                    let last = db.LoginLogs
                                 .Where(l => l.UsuarioID == u.UsuarioID)
                                 .OrderByDescending(l => l.FechaLogin)
                                 .FirstOrDefault()
                    orderby u.Nombre
                    select new
                    {
                        u.UsuarioID,
                        u.Nombre,
                        u.Rol,
                        FechaLogin = (DateTime?)(last != null ? last.FechaLogin : null),
                        FechaLogout = (DateTime?)(last != null ? last.FechaLogout : null)
                    }
                ).ToList();

                // ---- DataTable editable ----
                _dtUsuarios = new DataTable();
                _dtUsuarios.Columns.Add("UsuarioID", typeof(int));
                _dtUsuarios.Columns.Add("Nombre", typeof(string));
                _dtUsuarios.Columns.Add("Rol", typeof(string));
                _dtUsuarios.Columns.Add("FechaLogin", typeof(DateTime));
                _dtUsuarios.Columns.Add("FechaLogout", typeof(DateTime));

                _bsUsuarios.DataSource = _dtUsuarios;
                dgvUsuarios.AutoGenerateColumns = true;
                dgvUsuarios.DataSource = _bsUsuarios;

                foreach (var x in data)
                {
                    var r = _dtUsuarios.NewRow();
                    r["UsuarioID"] = x.UsuarioID;
                    r["Nombre"] = x.Nombre ?? "";
                    r["Rol"] = x.Rol ?? "";
                    r["FechaLogin"] = x.FechaLogin.HasValue ? x.FechaLogin.Value : (object)DBNull.Value;
                    r["FechaLogout"] = x.FechaLogout.HasValue ? x.FechaLogout.Value : (object)DBNull.Value;
                    _dtUsuarios.Rows.Add(r);
                }

                dgvUsuarios.AutoGenerateColumns = true;
                dgvUsuarios.DataSource = _dtUsuarios;

                // Encabezados y formatos
                if (dgvUsuarios.Columns["UsuarioID"] != null) dgvUsuarios.Columns["UsuarioID"].HeaderText = "ID";
                if (dgvUsuarios.Columns["Nombre"] != null) dgvUsuarios.Columns["Nombre"].HeaderText = "Nombre";
                if (dgvUsuarios.Columns["Rol"] != null) dgvUsuarios.Columns["Rol"].HeaderText = "Rol";
                if (dgvUsuarios.Columns["FechaLogin"] != null)
                {
                    dgvUsuarios.Columns["FechaLogin"].HeaderText = "Fecha Login";
                    dgvUsuarios.Columns["FechaLogin"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }
                if (dgvUsuarios.Columns["FechaLogout"] != null)
                {
                    dgvUsuarios.Columns["FechaLogout"].HeaderText = "Fecha Logout";
                    dgvUsuarios.Columns["FechaLogout"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                }

                // Habilitar edición solo en Nombre y Rol
                dgvUsuarios.ReadOnly = false;
                dgvUsuarios.Columns["UsuarioID"].ReadOnly = true;
                dgvUsuarios.Columns["FechaLogin"].ReadOnly = true;
                dgvUsuarios.Columns["FechaLogout"].ReadOnly = true;

                // Columna Rol como ComboBox con 3 opciones
                int idxRol = dgvUsuarios.Columns["Rol"].Index;
                dgvUsuarios.Columns.RemoveAt(idxRol);
                var colRol = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "Rol",
                    HeaderText = "Rol",
                    Name = "Rol",
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat,
                    ValueType = typeof(string)
                };
                colRol.Items.AddRange("Digitador", "Gerente", "Cajero");
                dgvUsuarios.Columns.Insert(idxRol, colRol);

                dgvUsuarios.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dgvUsuarios.AllowUserToAddRows = false;
                dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadUsuarios()
        {
            using var db = DbContextFactory.Create();
            var list = db.Usuario.Select(u => new { u.UsuarioID, u.Nombre, u.Rol, u.FechaCreacion }).ToList();
            dgvUsuarios.DataSource = list;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return;
            int id = (int)dgvUsuarios.SelectedRows[0].Cells["UsuarioID"].Value;
            using var db = DbContextFactory.Create();
            var u = db.Usuario.Find(id);
            if (u == null) return;
            if (MessageBox.Show("Eliminar usuario?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.Usuario.Remove(u); db.SaveChanges();
                LoadUsuarios();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_dtUsuarios == null)
            {
                MessageBox.Show("Primero pulsa Mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Validate(); // dispara validaciones y cierra ediciones pendientes
            dgvUsuarios.EndEdit(); // cierra el editor de la celda actual (textbox, combo, etc.)
            dgvUsuarios.CommitEdit(DataGridViewDataErrorContexts.Commit); // por si es combo
            _bsUsuarios.EndEdit(); // cierra edición del BindingSource
            var cm = this.BindingContext?[_bsUsuarios] as CurrencyManager;
            cm?.EndCurrentEdit();

            try
            {
                using var db = DbContextFactory.Create();

                foreach (DataRow row in _dtUsuarios.Rows)
                {
                    if (row.RowState != DataRowState.Modified) continue;

                    int id = Convert.ToInt32(row["UsuarioID"]);
                    string nombre = (row["Nombre"] ?? "").ToString();
                    string rol = (row["Rol"] ?? "").ToString();

                    // Validar rol
                    if (rol != "Digitador" && rol != "Gerente" && rol != "Cajero")
                        throw new InvalidOperationException($"Rol inválido en UsuarioID {id}. Usa: Digitador, Gerente o Cajero.");

                    var u = db.Usuario.Find(id);
                    if (u == null) continue;

                    u.Nombre = nombre;
                    u.Rol = rol;
                    db.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                db.SaveChanges();
                _dtUsuarios.AcceptChanges();
                MessageBox.Show("Cambios guardados.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error guardando: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
    }
}

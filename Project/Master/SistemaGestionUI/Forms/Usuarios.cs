﻿using SistemaGestionBussiness;
using SistemaGestionEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaGestionUI.Forms
{
    public partial class Usuarios : Form
    {
        #region Visuals
        public Color gradientTopLeftColor = Color.FromArgb(168, 197, 69);
        public Color gradientBottomLeftColor = Color.FromArgb(11, 57, 84);
        public Color gradientTopRightColor = Color.FromArgb(77, 33, 173);
        public Color gradientBottomRightColor = Color.FromArgb(43, 24, 82);

        Bitmap Gradient2D(Rectangle r, Color c1, Color c2, Color c3, Color c4)
        {
            Bitmap bmp = new Bitmap(r.Width, r.Height);

            float delta12R = 1f * (c2.R - c1.R) / r.Height;
            float delta12G = 1f * (c2.G - c1.G) / r.Height;
            float delta12B = 1f * (c2.B - c1.B) / r.Height;
            float delta34R = 1f * (c4.R - c3.R) / r.Height;
            float delta34G = 1f * (c4.G - c3.G) / r.Height;
            float delta34B = 1f * (c4.B - c3.B) / r.Height;
            using (Graphics G = Graphics.FromImage(bmp))
                for (int y = 0; y < r.Height; y++)
                {
                    Color c12 = Color.FromArgb(255, c1.R + (int)(y * delta12R),
                          c1.G + (int)(y * delta12G), c1.B + (int)(y * delta12B));
                    Color c34 = Color.FromArgb(255, c3.R + (int)(y * delta34R),
                          c3.G + (int)(y * delta34G), c3.B + (int)(y * delta34B));
                    using (LinearGradientBrush lgBrush = new LinearGradientBrush(
                          new Rectangle(0, y, r.Width, 1), c12, c34, 0f))
                    { G.FillRectangle(lgBrush, 0, y, r.Width, 1); }
                }
            return bmp;
        }
        #endregion

        public Usuarios()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

        }
        private void Usuarios_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Gradient2D(this.ClientRectangle, gradientTopLeftColor, gradientBottomLeftColor, gradientTopRightColor, gradientBottomRightColor);
            GetUsuarios();
        }

        void GetUsuarios()
        {
            pUser.Visible = false;
            btnEjecutar.Visible = false;
            lblCurrentCommand.Text = string.Empty;
            lblIDEspecifico.Visible = false;
            tbIDEspecifico.Visible = false;
            cbCommands.SelectedItem = 0;
            cbCommands.Text = "-Seleccionar Comando-";

            try
            {
                List<Usuario> usuarios = UsuarioBussiness.Bussiness_ListarUsuarios();
                dgvUsuarios.DataSource = usuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esto traerá nuevamente a todos los usuarios, ¿continuar?.", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                GetUsuarios();
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                string? current = cbCommands.SelectedItem.ToString();
                string? specificID = tbIDEspecifico.Text;
                int _id = 0;

                if (current.Contains("Obtener") || current.Contains("Modificar") || current.Contains("Eliminar"))
                {
                    if (!int.TryParse(specificID, out _id))
                    {
                        MessageBox.Show("Ingrese un ID valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                switch (current)
                {
                    case "Obtener Usuario":
                        List<Usuario> usuario = UsuarioBussiness.Bussiness_ObtenerUsuario(_id);
                        dgvUsuarios.DataSource = usuario;
                        break;

                    case "Crear Usuario":

                        if (MessageBox.Show("Esta acción añadirá al nuevo usuario de la data base, ¿desea continuar?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            Usuario usuarioNuevo = new Usuario
                            {
                                Nombre = tbNombre.Text,
                                Apellido = tbApellido.Text,
                                NombreUsuario = tbUserName.Text,
                                Contrasenia = tbContrasenia.Text,
                                Mail = tbEmail.Text,
                            };
                            UsuarioBussiness.Bussiness_CrearUsuario(usuarioNuevo);
                            MessageBox.Show("Usuario Correctamente Creado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Usuario No Creado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        GetUsuarios();
                        break;

                    case "Modificar Usuario":

                        if (MessageBox.Show("Esta acción modificara el usuario en la data base, ¿desea continuar?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            Usuario usuarioModificado = new Usuario
                            {
                                Id = _id,
                                Nombre = tbNombre.Text,
                                Apellido = tbApellido.Text,
                                NombreUsuario = tbUserName.Text,
                                Contrasenia = tbContrasenia.Text,
                                Mail = tbEmail.Text,
                            };
                            UsuarioBussiness.Bussiness_ModificarUsuario(usuarioModificado);
                            MessageBox.Show("Usuario Correctamente Modificado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Usuario No Modificado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        GetUsuarios();
                        break;

                    case "Eliminar Usuario":

                        if (MessageBox.Show("Esta acción eliminará el usuario de la data base, ¿desea continuar?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            Usuario usuarioAEliminar = new Usuario { Id = _id };
                            UsuarioBussiness.Bussiness_EliminarUsuario(usuarioAEliminar);
                            MessageBox.Show("Usuario Correctamente Eliminado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Usuario No Eliminado", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        GetUsuarios();
                        break;

                    default:
                        MessageBox.Show("No Valido!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cbCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            pUser.Visible = false;

            tbNombre.ReadOnly = false;
            tbApellido.ReadOnly = false;
            tbUserName.ReadOnly = false;
            tbContrasenia.ReadOnly = false;
            tbEmail.ReadOnly = false;

            tbNombre.Text = String.Empty;
            tbApellido.Text = String.Empty;
            tbUserName.Text = String.Empty;
            tbContrasenia.Text = String.Empty;
            tbEmail.Text = String.Empty;

            tbIDEspecifico.ReadOnly = false;
            tbIDEspecifico.Visible = false;
            tbIDEspecifico.Text = String.Empty;
            lblIDEspecifico.Visible = false;

            string current = cbCommands.SelectedItem.ToString();
            lblCurrentCommand.Text = $"Current Command: {current}";


            switch (current)
            {
                case "Obtener Usuario":
                    tbIDEspecifico.Visible = true;
                    lblIDEspecifico.Visible = true;
                    break;
                case "Crear Usuario":
                    pUser.Visible = true;
                    break;
                case "Modificar Usuario":
                    pUser.Visible = true;
                    tbIDEspecifico.Visible = true;
                    lblIDEspecifico.Visible = true;
                    break;
                case "Eliminar Usuario":
                    pUser.Visible = true;
                    tbIDEspecifico.Visible = true;
                    lblIDEspecifico.Visible = true;

                    tbNombre.ReadOnly = true;
                    tbApellido.ReadOnly = true;
                    tbUserName.ReadOnly = true;
                    tbContrasenia.ReadOnly = true;
                    tbEmail.ReadOnly = true;
                    break;

                default: break;
            }

            if (String.IsNullOrEmpty(current) || current == "-Seleccionar Comando-")
            {
                btnEjecutar.Visible = false;
            }
            else
            {
                btnEjecutar.Visible = true;
            }
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cbCommands.SelectedItem.ToString()))
                {
                    if (dgvUsuarios.SelectedRows.Count > 0)
                    {
                        var selectedRow = dgvUsuarios.SelectedRows[0];
                        tbIDEspecifico.Text = selectedRow.Cells["Id"].Value.ToString();
                        tbIDEspecifico.ReadOnly = true;
                        string current = cbCommands.SelectedItem.ToString();

                        switch (current)
                        {
                            case "Modificar Usuario":
                            case "Eliminar Usuario":
                                tbNombre.Text = selectedRow.Cells["Nombre"].Value.ToString();
                                tbApellido.Text = selectedRow.Cells["Apellido"].Value.ToString();
                                tbUserName.Text = selectedRow.Cells["NombreUsuario"].Value.ToString();
                                tbContrasenia.Text = selectedRow.Cells["Contrasenia"].Value.ToString();
                                tbEmail.Text = selectedRow.Cells["Mail"].Value.ToString();
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void tbIDEspecifico_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

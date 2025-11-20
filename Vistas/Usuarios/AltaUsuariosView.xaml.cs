using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ClasesBase; // 👈 AGREGAR ESTA LÍNEA para acceder a TrabajarUsuarios
using ClasesBase.Entidades;
using ClasesBase.Repositories;
using ClasesBase.Services;

namespace Vistas.Usuarios
{
    /// <summary>
    /// Interaction logic for AltaUsuariosView.xaml
    /// </summary>
    public partial class AltaUsuariosView : Window
    {
        RolRepository rolRep = new RolRepository();
        private UsuarioService _service = new UsuarioService();

        // CORRECCIÓN: Se añaden las declaraciones faltantes (Errores 21 al 29)
        private bool _modoEdicion = false;
        private Usuario _usuarioActual;

        public AltaUsuariosView()
        {
            InitializeComponent();
            CargarRoles();
        }

        // Constructor para modificación
        public AltaUsuariosView(Usuario usuario)
            : this()
        {
            _modoEdicion = true;
            _usuarioActual = usuario;

            // Precargar los campos
            txtUsuario.Text = usuario.Usu_NombreUsuario;
            txtPassword.Password = usuario.Usu_Contrasenia;
            txtApellidoNombre.Text = usuario.Usu_ApellidoNombre;
            cmbRol.SelectedValue = usuario.Rol_ID;
        }

        private void CargarRoles()
        {
            // Limpiar cualquier item existente primero
            cmbRol.Items.Clear();

            // Cargar roles directamente desde el Repository
            cmbRol.ItemsSource = RolRepository.ObtenerTodos();
            cmbRol.DisplayMemberPath = "Rol_Descripcion";
            cmbRol.SelectedValuePath = "Rol_ID";
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            if (_modoEdicion)
            {
                _usuarioActual.Usu_NombreUsuario = txtUsuario.Text;
                _usuarioActual.Usu_Contrasenia = txtPassword.Password;
                _usuarioActual.Usu_ApellidoNombre = txtApellidoNombre.Text;
                _usuarioActual.Rol_ID = (int)cmbRol.SelectedValue;

                try
                {
                    _service.ActualizarUsuario(_usuarioActual);

                    MessageBox.Show("Usuario modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true; // ✅ IMPORTANTE
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtUsuario.Focus();
                    txtUsuario.SelectAll();
                }
                return;
            }

            // --- ALTA ---
            int rolId = (int)cmbRol.SelectedValue;
            string rolDescripcion = ObtenerDescripcionRol(rolId);

            Usuario nuevoUsuario = new Usuario
            {
                Usu_NombreUsuario = txtUsuario.Text,
                Usu_Contrasenia = txtPassword.Password,
                Usu_ApellidoNombre = txtApellidoNombre.Text,
                Rol_ID = rolId
            };

            string mensajeConfirmacion = "DATOS DEL USUARIO A CREAR\n\n" +
                                            "Usuario: " + nuevoUsuario.Usu_NombreUsuario + "\n" +
                                            "Contraseña: " + new string('*', nuevoUsuario.Usu_Contrasenia.Length) + "\n" +
                                            "Nombre: " + nuevoUsuario.Usu_ApellidoNombre + "\n" +
                                            "Rol: " + rolDescripcion + "\n" +
                                            "¿Confirmar la creación de este usuario?";

            MessageBoxResult result = MessageBox.Show(
                mensajeConfirmacion,
                "CONFIRMAR ALTA DE USUARIO",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _service.GuardarUsuario(nuevoUsuario);

                    string mensajeExito = "USUARIO CREADO EXITOSAMENTE\n\n" +
                                            "Usuario: " + nuevoUsuario.Usu_NombreUsuario + "\n" +
                                            "Rol: " + rolDescripcion;

                    MessageBox.Show(mensajeExito, "ALTA EXITOSA", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarCampos();
                    this.DialogResult = true; // ✅ IMPORTANTE: incluso después de limpiar, si se creó, hay cambio
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtUsuario.Focus();
                    txtUsuario.SelectAll();
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // explícito
            this.Close();
        }


        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("El campo Usuario es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("El campo Contraseña es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellidoNombre.Text))
            {
                MessageBox.Show("El campo Apellido y Nombre es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtApellidoNombre.Focus();
                return false;
            }

            if (cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un Rol", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbRol.Focus();
                return false;
            }

            return true;
        }

        private string ObtenerDescripcionRol(int rolId)
        {
            Rol rol = rolRep.Obtener(rolId);
            return rol != null ? rol.Rol_Descripcion : "Desconocido";
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            txtApellidoNombre.Clear();
            cmbRol.SelectedIndex = -1;
            txtUsuario.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}